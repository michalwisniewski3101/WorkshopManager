<template>
  <v-card style="height: 100%; width: 100%;">
    <v-tabs v-model="tab" bg-color="primary">
      <v-tab value="specialties">Elementy Magazynu</v-tab>
      <v-tab value="services">Nowa dostawa</v-tab>
    </v-tabs>





    <v-tabs-window v-model="tab">
      <v-tabs-window-item value="specialties">
        <v-row>
          <v-col cols="12">
            <h2>Elementy Magazynu</h2>
            <v-data-table :headers="headers" :items="inventoryItems" item-value="id" class="elevation-1">
              <template #item.actions="{ item }">
                <v-btn icon @click="editItem(item)">
                  <v-icon>mdi-pencil</v-icon>
                </v-btn>
                <v-btn icon color="red" @click="deleteItem(item.id)">
                  <v-icon>mdi-delete</v-icon>
                </v-btn>
              </template>
            </v-data-table>
          </v-col>

          <v-col cols="12" class="text-right">
            <v-btn @click="openInventoryModal" color="primary">Dodaj nowy element magazynu</v-btn>
          </v-col>
        </v-row>

        <!-- Modal dodawania elementu magazynu -->
        <v-dialog v-model="showInventoryModal" persistent max-width="500">
          <v-card>
            <v-card-title class="text-h6">Dodaj Element Magazynu</v-card-title>
            <v-card-text>
              <v-form @submit.prevent="addInventoryItem">
                <v-text-field v-model="newInventoryItem.name" label="Nazwa" required></v-text-field>
                <v-text-field v-model="newInventoryItem.description" label="Opis" required></v-text-field>
                <v-text-field v-model="newInventoryItem.productNumber" label="Numer produktu" required></v-text-field>
                <v-text-field v-model.number="newInventoryItem.unitPrice" label="Cena jednostkowa (PLN)" type="number"
                  required></v-text-field>
                <v-text-field v-model.number="newInventoryItem.reorderLevel" label="Poziom zamawiania" type="number"
                  required></v-text-field>
                <v-text-field v-model="newInventoryItem.supplier" label="Dostawca" required></v-text-field>
              </v-form>
            </v-card-text>
            <v-card-actions>
              <v-btn color="primary" @click="addInventoryItem">Zapisz</v-btn>
              <v-btn color="grey" @click="closeInventoryModal">Anuluj</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
  
    </v-tabs-window-item>

  <v-tabs-window-item value="services">
    
  </v-tabs-window-item>
  </v-tabs-window>
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
  { title: 'Akcje', key: 'actions', sortable: false }
]

const inventoryItems = ref([])
const newInventoryItem = ref({
  name: '',
  description: '',
  productNumber: '',
  unitPrice: 0,
  reorderLevel: 0,
  supplier: ''
})
const showInventoryModal = ref(false)

const openInventoryModal = () => (showInventoryModal.value = true)
const closeInventoryModal = () => {
  showInventoryModal.value = false
  newInventoryItem.value = {
    name: '',
    description: '',
    productNumber: '',
    unitPrice: 0,
    reorderLevel: 0,
    supplier: ''
  }
}

const fetchInventoryItems = async () => {
  try {
    inventoryItems.value = await $fetch('/api/InventoryItem')
  } catch (error) {
    console.error('Błąd podczas ładowania elementów magazynu', error)
  }
}

const addInventoryItem = async () => {
  try {
    await $fetch('/api/InventoryItem', {
      method: 'POST',
      body: JSON.stringify(newInventoryItem.value),
      headers: { 'Content-Type': 'application/json' }
    })
    await fetchInventoryItems()
    closeInventoryModal()
  } catch (error) {
    console.error('Błąd podczas dodawania elementu magazynu', error)
  }
}

const editItem = (item) => {
  // Logika edycji elementu
  console.log('Edycja elementu', item)
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
</style>
