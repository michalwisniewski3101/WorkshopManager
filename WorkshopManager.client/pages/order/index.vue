<template>
  <v-card style="height: 100%; width: 100%;">
    <v-breadcrumbs :items="items">
      </v-breadcrumbs>
      <v-row>
      <v-col cols="12" class="text-left">
        <h1>Lista zamówień</h1>
      </v-col>
      <v-col cols="4" class="text-left">
        <NuxtLink to="/order/create">
      <v-btn style="background-color: #4caf50;" >
        <v-icon>mdi-plus</v-icon>Dodaj nowe zamówienie
      </v-btn>
    </NuxtLink>
      </v-col>
      <v-col cols="4" class="text-left">
        <v-select variant="outlined"
          v-model="selectedStatus"
          :items="statusOptions"
          label="Filtruj po statusie"
          item-title="text"
          item-value="value"
          @change="filterOrders"
          clearable
        ></v-select>
      </v-col>
      <v-col cols="4" class="text-right">
        <v-text-field variant="outlined"
        v-model="search"
        label="Szukaj"
        prepend-inner-icon="mdi-magnify"
        hide-details
        single-line
      ></v-text-field>
      </v-col>
    </v-row>
    <v-data-table
      v-if="orders.length"
      :headers="tableHeaders"
      :items="orders"
      item-key="id"
      class="elevation-1"
      :search="search"
    >
      <template v-slot:item.status="{ item }">
        {{ getOrderStatusName(item.orderStatus) }}
      </template>

      <template v-slot:item.orderDate="{ item }">
        {{ new Date(item.orderDate).toLocaleDateString() }}
      </template>

      <template v-slot:item.clientName="{ item }">
        {{ item.clientName }} - {{ item.clientPhoneNumber }}
      </template>

      <template v-slot:item.actions="{ item }">
        <NuxtLink :to="`/order/${item.id}`" class="clickable-icon">
         

  <v-icon>mdi-eye</v-icon>

        </NuxtLink>
        <v-icon @click="editItem(item)" class="clickable-icon">mdi-pencil</v-icon>
        <v-icon @click="deleteItem(item.id)" class="clickable-icon">mdi-delete</v-icon>
      </template>
    </v-data-table>
  </v-card>
</template>

<script setup>
import { ref, onMounted } from 'vue'

const orders = ref([])

const tableHeaders = [
  { title: 'Numer zamówienia', key: 'description' },
  { title: 'Data zamówienia', key: 'orderDate' },
  { title: 'Klient', key: 'clientName' },
  { title: 'Status', key: 'status' },
  { title: 'Akcje', key: 'actions', sortable: false }
]
const search = ref("")
const items = ref([
  {
    title: 'Dashboard',
    disabled: false,
    to: '/',
  },
  {
    title: 'Zamówienia',
    disabled: true,
    to: 'order',
  },
]);
const getOrderStatusName = (status) => {
  const statuses = {
    0: 'Nowe',
    1: 'W trakcie realizacji',
    2: 'Oczekiwanie na zatwierdzenie',
    3: 'Zakończone',
    4: 'Wstrzymane',
    5: 'Anulowane'
  }
  return statuses[status] || 'Nieznany status'
}

const fetchOrders = async () => {
  try {
    orders.value = await $fetch('/api/Order')
  } catch (error) {
    alert('Błąd podczas ładowania listy zamówień')
  }
}

onMounted(fetchOrders)
</script>

<style scoped>
/* Dostosowanie do stylu Vuetify */
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
}

.v-text-field {
  max-width: 300px;
  margin-left: auto;
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

:deep(.v-data-table-footer) {
  background: rgb(185, 185, 185);
  color: #000000;
}
:deep(.v-pagination .v-btn:not(.v-btn--active)) { 
  background-color: #c0c0c0 !important; 
  color: #000000 !important; 
  margin: 0 2px; 
}


:deep(.v-pagination .v-pagination__item--active .v-btn) { 
  background-color: #4caf50 !important; 
  color: white !important; 
}


:deep(.v-pagination .v-pagination__navigation .v-btn[disabled]) {
  background-color: #e0e0e0 !important; 
  color: #a0a0a0 !important; 
  opacity: 0.6; 
}

:deep(.v-pagination .v-btn:not(.v-btn--active):not([disabled]):hover) {
  background-color: #a8a8a8 !important; 
}
</style>
