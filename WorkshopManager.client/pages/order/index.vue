<template>
  <v-card style="height: 100%; width: 100%;">
    <v-breadcrumbs :items="items">
      </v-breadcrumbs>
      <v-row>
      <v-col cols="12" class="text-left">
        <h1>Lista zamówień</h1>
      </v-col>
      <v-col cols="6" class="text-left">
        <NuxtLink to="/order/create">
      <v-btn color="primary" >
        Dodaj nowe zamówienie
      </v-btn>
    </NuxtLink>
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
      </template>
    </v-data-table>
  </v-card>
</template>

<script setup>
import { ref, onMounted } from 'vue'

const orders = ref([])

const tableHeaders = [
  { title: 'Zamówienie ID', key: 'id' },
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
}

h1 {
  color: var(--v-primary-base); /* Kolor z motywu Vuetify */
}
</style>
