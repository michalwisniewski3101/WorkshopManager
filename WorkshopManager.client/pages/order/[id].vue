<template>
    <div>
      <h1>Szczegóły zamówienia</h1>
      <div v-if="order">
        <p><strong>ID:</strong> {{ order.id }}</p>
        <p><strong>Data zamówienia:</strong> {{ new Date(order.orderDate).toLocaleDateString() }}</p>
        <p><strong>Klient:</strong> {{ order.clientName }}</p>
        <p><strong>Telefon:</strong> {{ order.clientPhoneNumber }}</p>
        <p><strong>Opis:</strong> {{ order.description }}</p>
        <p><strong>Status:</strong> {{ getOrderStatusName(order.orderStatus) }}</p>
        <p>
          <strong>Szacowana data ukończenia:</strong>
          {{ order.estimatedCompletionDate
            ? new Date(order.estimatedCompletionDate).toLocaleDateString()
            : 'Brak' }}
        </p>
        <p>
          <strong>Całkowity koszt:</strong>
          {{ order.totalCost ? `${order.totalCost} PLN` : 'Nie ustalono' }}
        </p>
      </div>
      <NuxtLink to="/order"><button>Wróć do listy zamówień</button></NuxtLink>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue'
  import { useRoute } from 'vue-router'
  
  const order = ref(null)
  const route = useRoute()
  
  const getOrderStatusName = (status) => {
    const statuses = {
      0: 'Nowe',
      1: 'W trakcie realizacji',
      2: 'Zakończone',
      3: 'Anulowane'
    }
    return statuses[status] || 'Nieznany status'
  }
  
  const fetchOrderDetails = async () => {
    try {
      order.value = await $fetch(`/api/Order/${route.params.id}`)
    } catch (error) {
      alert('Błąd podczas ładowania szczegółów zamówienia')
    }
  }
  
  onMounted(fetchOrderDetails)
  </script>
  