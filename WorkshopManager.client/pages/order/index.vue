<template>
    <div>
      <h1>Lista zamówień</h1>
      <ul>
        <li v-for="order in orders" :key="order.id">
          <strong>Zamówienie ID: {{ order.id }}</strong>
          <p>Data zamówienia: {{ new Date(order.orderDate).toLocaleDateString() }}</p>
          <p>Klient: {{ order.clientName }} - Telefon: {{ order.clientPhoneNumber }}</p>
          <p>Status: {{ getOrderStatusName(order.orderStatus) }}</p>
          <p>
            <NuxtLink :to="`/order/${order.id}`">Zobacz szczegóły</NuxtLink>
          </p>
        </li>
      </ul>
  
      <NuxtLink to="/order/create">
        <button>Dodaj nowe zamówienie</button>
      </NuxtLink>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue'
  
  const orders = ref([])
  
  const getOrderStatusName = (status) => {
    const statuses = {
      0: 'Nowe',
      1: 'W trakcie realizacji',
      2: 'Zakończone',
      3: 'Anulowane'
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
  