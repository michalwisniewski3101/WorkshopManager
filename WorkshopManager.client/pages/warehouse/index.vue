<template>
  <v-card style="height: 100%; width: 100%;">
    <v-breadcrumbs :items="items">
    </v-breadcrumbs>
    <v-row>
      <v-col cols="12" class="text-left">
        <h1>Elementy Magazynu</h1>
      </v-col>
      <v-col cols="6" class="text-left">
        <v-btn style="background-color: #4caf50;" @click="openInventoryModal" ><v-icon>mdi-plus</v-icon>Dodaj nowy element magazynu</v-btn>
      </v-col>
      <v-col cols="6" class="text-right">
        <v-text-field
        v-model="search"
        label="Szukaj"
        prepend-inner-icon="mdi-magnify"
        variant="outlined"
        hide-details
        single-line
      ></v-text-field>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="12">
        <v-data-table :headers="headers" :items="inventoryItems" item-value="id" class="elevation-1" :search="search">
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
    <v-dialog v-model="showInventoryModal" persistent max-width="800">
      <v-card>
        <v-card-title >{{ isEditMode ? 'Edytuj Element Magazynu' : 'Dodaj Element Magazynu' }}</v-card-title>
        <v-card-text>
          <v-form @submit.prevent="saveInventoryItem">
            <v-row>
              <v-col>
                <v-text-field v-model="newInventoryItem.name" label="Nazwa" required></v-text-field>
            <v-text-field v-model="newInventoryItem.description" label="Opis" required></v-text-field>
            <v-text-field v-model="newInventoryItem.productNumber" label="Numer produktu" required></v-text-field>
              </v-col>
              <v-col>
                <v-text-field v-model.number="newInventoryItem.unitPrice" label="Cena jednostkowa (PLN)" type="number" required></v-text-field>
            <v-text-field v-model.number="newInventoryItem.reorderLevel" label="Poziom zamawiania" type="number" required></v-text-field>
            <v-text-field v-model="newInventoryItem.supplier" label="Dostawca" required></v-text-field>
              </v-col>
            </v-row>


            <v-select
              v-model.number="newInventoryItem.taxRate"
              :items="[23, 8, 5, 0]"
              label="Stawka Podatku"
              required
            ></v-select>
          </v-form>
        </v-card-text>
        <v-card-actions>
          
          <v-btn style="background-color: #f15c5c;" @click="closeInventoryModal">Anuluj</v-btn>
          <v-btn style="background-color: #4caf50;" @click="saveInventoryItem">Zapisz</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Modal aktualizacji ilości w magazynie -->
    <v-dialog v-model="showQuantityModal" persistent max-width="800">
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
          
          <v-btn style="background-color: #f15c5c;" @click="closeQuantityModal">Anuluj</v-btn>
          <v-btn style="background-color: #4caf50;"  @click="updateQuantity">Zapisz</v-btn>
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
const items = ref([
  {
    title: 'Dashboard',
    disabled: false,
    to: '/',
  },
  {
    title: 'Magazyn',
    disabled: true,
    to: 'warehouse/',
  },
]);
const search = ref("")
const headers = [
  { title: 'Nazwa', key: 'name' },
  { title: 'Opis', key: 'description' },
  { title: 'Numer produktu', key: 'productNumber' },
  { title: 'Cena jednostkowa (PLN)', key: 'unitPrice' },
  { title: 'Ilość w magazynie', key: 'quantityInStock' },
  { title: 'Stawka podatku', key: 'taxRate' },
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
  supplier: '',
  taxRate: 23
})
const selectedInventoryItem = ref({}) 
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
    supplier: '',
    taxRate: 23
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
const updateQuantity = async () => {
  try {
    const { id}  = selectedInventoryItem.value
    await $fetch(`/api/InventoryItem/UpdateQuantityInStock/${id}?quantity=${addedQuantity.value}`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' }
    })
    await fetchInventoryItems()
    closeQuantityModal()
  } catch (error) {
    console.error('Błąd podczas aktualizacji ilości w magazynie', error)
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
.v-btn {
  margin-top: 1rem;
  transition: background 0.3s ease;
}

h1 {
  color: #000000;
  margin-bottom: 1rem;
  font-family: 'Arial', sans-serif;
}

.v-card {
  padding: 20px;
  background: #e0e0e0;
  border-radius: 10px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1) !important;
  color:#000000;
}

.v-text-field {
  color: #000000;
}

.clickable-icon {
  color: #000000;
  transition: color 0.3s ease;
}

.clickable-icon:hover {
  color: #45a049;
}

:deep(.v-data-table) {
  background: rgb(185, 185, 185);
  border-radius: 5px;
  margin-top: 1rem;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.05) !important;
  color: #000000;
}

:deep(.v-data-table th) {
  font-weight: bold;
  color: #000000;
  background: #a0a0a0;
}

:deep(.v-data-table td) {
  color: #000000;
}
.v-breadcrumbs:deep() {
  color: #000000
}
</style>
