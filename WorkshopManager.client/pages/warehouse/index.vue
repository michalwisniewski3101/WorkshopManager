<template>
    <div>
      <!-- Sekcja Magazyn -->
      <section>
        <h2>Elementy Magazynu</h2>
        <v-data-table
          :headers="headers"
          :items="inventoryItems"
          item-value="id"
          class="elevation-1"
        >
          <template #item.actions="{ item }">
            <v-btn icon @click="editItem(item)">
              <v-icon>mdi-pencil</v-icon>
            </v-btn>
            <v-btn icon color="red" @click="deleteItem(item.id)">
              <v-icon>mdi-delete</v-icon>
            </v-btn>
          </template>
        </v-data-table>
        <v-btn @click="openInventoryModal" color="primary">Dodaj nowy element magazynu</v-btn>
      </section>
  
      <!-- Modal dodawania elementu magazynu -->
      <dialog model="showInventoryModal" max-width="500">
        <v-card>
          <v-card-title>Dodaj Element Magazynu</v-card-title>
          <v-card-text>
            <form @submit.prevent="addInventoryItem">
              <v-text-field v-model="newInventoryItem.name" label="Nazwa" required></v-text-field>
              <v-text-field v-model="newInventoryItem.description" label="Opis" required></v-text-field>
              <v-text-field v-model="newInventoryItem.productNumber" label="Numer produktu" required></v-text-field>
              <v-text-field v-model.number="newInventoryItem.unitPrice" label="Cena jednostkowa (PLN)" type="number" required></v-text-field>
              <v-text-field v-model.number="newInventoryItem.reorderLevel" label="Poziom zamawiania" type="number" required></v-text-field>
              <v-text-field v-model="newInventoryItem.supplier" label="Dostawca" required></v-text-field>
            </form>
          </v-card-text>
          <v-card-actions>
            <v-btn color="primary" @click="addInventoryItem">Zapisz</v-btn>
            <v-btn color="grey" @click="closeInventoryModal">Anuluj</v-btn>
          </v-card-actions>
        </v-card>
      </dialog>
    </div>
  </template>
  <script setup>
  import { ref, onMounted } from 'vue'
  
  // Definiowanie meta danych
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
    { title: 'Poziom zamawiania', key: 'reorderLevel' },
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
  