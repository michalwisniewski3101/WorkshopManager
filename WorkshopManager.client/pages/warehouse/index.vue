<template>
  <v-card style="height: 100%; width: 100%;">
    <v-row>
      <v-col cols="6" class="text-left">
        <h2>Elementy Magazynu</h2>
      </v-col>
      <v-col cols="6" class="text-right">
        <v-btn @click="openInventoryModal" color="primary">Dodaj nowy element magazynu</v-btn>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="12">
        <v-data-table :headers="headers" :items="inventoryItems" item-value="id" class="elevation-1">
          <template #item.actions="{ item }">
            <v-icon @click="editItem(item)" class="clickable-icon">mdi-pencil</v-icon>
            <v-icon @click="deleteItem(item.id)" class="clickable-icon">mdi-delete</v-icon>
          </template>
          <template #item.quantityInStock="{ item }">
            {{ item.quantityInStock }}
            <v-icon @click="openQuantityModal(item)" class="clickable-icon">mdi-plus</v-icon>
          </template>
        </v-data-table>
      </v-col>
    </v-row>

    <!-- Modal dodawania lub edycji elementu magazynu -->
    <v-dialog v-model="showInventoryModal" persistent max-width="500">
      <v-card>
        <v-card-title class="text-h6">{{ isEditMode ? 'Edytuj Element Magazynu' : 'Dodaj Element Magazynu' }}</v-card-title>
        <v-card-text>
          <v-form @submit.prevent="saveInventoryItem">
            <v-text-field v-model="newInventoryItem.name" label="Nazwa" required></v-text-field>
            <v-text-field v-model="newInventoryItem.description" label="Opis" required></v-text-field>
            <v-text-field v-model="newInventoryItem.productNumber" label="Numer produktu" required></v-text-field>
            <v-text-field v-model.number="newInventoryItem.unitPrice" label="Cena jednostkowa (PLN)" type="number" required></v-text-field>
            <v-text-field v-model.number="newInventoryItem.reorderLevel" label="Poziom zamawiania" type="number" required></v-text-field>
            <v-text-field v-model="newInventoryItem.supplier" label="Dostawca" required></v-text-field>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-btn color="primary" @click="saveInventoryItem">Zapisz</v-btn>
          <v-btn color="grey" @click="closeInventoryModal">Anuluj</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Modal aktualizacji ilości w magazynie -->
    <v-dialog v-model="showQuantityModal" persistent max-width="500">
      <v-card>
        <v-card-title class="text-h6">Zmień ilość w magazynie</v-card-title>
        <v-card-text>
          <v-form @submit.prevent="updateQuantity">
            <v-text-field :model-value="selectedInventoryItem.quantityInStock + addedQuantity" label="Aktualna ilość w magazynie"
            type="number" readonly variant="outlined"></v-text-field>
            <v-text-field v-model.number="addedQuantity" label="Ilość z dostawy" type="number" required></v-text-field>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-btn color="primary" @click="updateQuantity">Zapisz</v-btn>
          <v-btn color="grey" @click="closeQuantityModal">Anuluj</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

  </v-card>
</template>

<script setup>
import { ref, onMounted } from 'vue'

definePageMeta({
  middleware: 'auth',
  auth: {
    roles: ['Administrator', 'Starszy Mechanik']
  }
})

const headers = [
  { title: 'Nazwa', key: 'name' },
  { title: 'Opis', key: 'description' },
  { title: 'Numer produktu', key: 'productNumber' },
  { title: 'Cena jednostkowa (PLN)', key: 'unitPrice' },
  { title: 'Ilość w magazynie', key: 'quantityInStock' },
  { title: 'Dostawca', key: 'supplier' },
  { title: 'Akcje', key: 'actions', sortable: false, width: '15%' }
]

const inventoryItems = ref([])
const addedQuantity = ref(0)
const newInventoryItem = ref({
  id: null,
  name: '',
  description: '',
  productNumber: '',
  unitPrice: 0,
  reorderLevel: 0,
  supplier: ''
})
const selectedInventoryItem = ref({}) // Element, którego ilość jest aktualizowana
const showInventoryModal = ref(false)
const showQuantityModal = ref(false)
const isEditMode = ref(false)

const openInventoryModal = () => {
  isEditMode.value = false
  newInventoryItem.value = {
    id: null,
    name: '',
    description: '',
    productNumber: '',
    unitPrice: 0,
    reorderLevel: 0,
    supplier: ''
  }
  showInventoryModal.value = true
}

const closeInventoryModal = () => {
  showInventoryModal.value = false
}

const openQuantityModal = (item) => {
  selectedInventoryItem.value = { ...item }
  showQuantityModal.value = true
}

const closeQuantityModal = () => {
  showQuantityModal.value = false
  selectedInventoryItem.value = {}
  addedQuantity.value = 0
}

const fetchInventoryItems = async () => {
  try {
    inventoryItems.value = await $fetch('/api/InventoryItem')
  } catch (error) {
    console.error('Błąd podczas ładowania elementów magazynu', error)
  }
}

const saveInventoryItem = async () => {
  try {
    const method = newInventoryItem.value.id ? 'PUT' : 'POST'
    const url = newInventoryItem.value.id 
      ? `/api/InventoryItem/${newInventoryItem.value.id}`
      : '/api/InventoryItem'

    await $fetch(url, {
      method,
      body: JSON.stringify(newInventoryItem.value),
      headers: { 'Content-Type': 'application/json' }
    })
    await fetchInventoryItems()
    closeInventoryModal()
  } catch (error) {
    console.error('Błąd podczas zapisywania elementu magazynu', error)
  }
}

const editItem = (item) => {
  isEditMode.value = true
  newInventoryItem.value = { ...item }
  showInventoryModal.value = true
}

const deleteItem = async (id) => {
  try {
    await $fetch(`/api/InventoryItem/${id}`, { method: 'DELETE' })
    await fetchInventoryItems()
  } catch (error) {
    console.error('Błąd podczas usuwania elementu magazynu', error)
  }
}

onMounted(fetchInventoryItems)
</script>

<style scoped>
.text-right {
  text-align: right;
}

.clickable-icon {
  cursor: pointer;
  transition: color 0.2s;
}

.clickable-icon:hover {
  color: #007bff;
}
</style>
