<template>
  <v-card>
  <div>
    <h1>Szczegóły zamówienia</h1>

    <!-- Okno do podawania kodu zamówienia -->
    <div>
      <label for="orderCode">Wprowadź kod zamówienia:</label>
      <input v-model="orderCode" id="orderCode" type="text" placeholder="Wprowadź kod" />
      <button @click="loadOrderDetails">Pokaż zamówienie</button>
    </div>

    <!-- Szczegóły zamówienia -->
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
      <div v-if="serviceSchedules && serviceSchedules.length > 0">
        <h2>Harmonogramy serwisów</h2>
        <div
          v-for="schedule in serviceSchedules"
          :key="schedule.id"
          class="service-schedule"
        >
          <p><strong>Harmonogram ID:</strong> {{ schedule.id }}</p>
          <p>
            <strong>Data rozpoczęcia:</strong>
            {{
              schedule.serviceDateStart
                ? new Date(schedule.serviceDateStart).toLocaleDateString()
                : 'Brak'
            }}
          </p>
          <p>
            <strong>Data zakończenia:</strong>
            {{
              schedule.serviceDateEnd
                ? new Date(schedule.serviceDateEnd).toLocaleDateString()
                : 'Brak'
            }}
          </p>
          <p><strong>Serwis ID:</strong> {{ schedule.serviceId }}</p>
          <p><strong>Status serwisu:</strong> {{ getServiceStatusName(schedule.serviceStatus) }}</p>
          <div>
            <h3>Pozycje zamówienia:</h3>
            <ul>
              <li v-for="item in schedule.orderItems" :key="item.inventoryItemId">
                <p><strong>Pozycja ID:</strong> {{ item.inventoryItemId }}</p>
                <p><strong>Ilość:</strong> {{ item.quantity }}</p>
                <p><strong>Całkowita cena:</strong> {{ item.totalPrice }} PLN</p>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  </div>
</v-card>
</template>

<script setup>
import { ref } from 'vue'

definePageMeta({
  middleware: 'auth',
  auth: {
    roles: ['Administrator', 'Klient'],
  },
  layout: 'login'
})

const order = ref(null)
const serviceSchedules = ref([])
const orderCode = ref('')

const getOrderStatusName = (status) => {
  const statuses = {
    0: 'Nowe',
    1: 'W trakcie realizacji',
    2: 'Zakończone',
    3: 'Anulowane',
  }
  return statuses[status] || 'Nieznany status'
}

const fetchOrderDetails = async (code) => {
  try {
    order.value = await $fetch(`/api/Order/ByCode/${code}`)
    if (order.value && order.value.id) {
      await fetchServiceSchedules(order.value.id)
    }
  } catch (error) {
    alert('Błąd podczas ładowania szczegółów zamówienia')
  }
}

const fetchServiceSchedules = async (id) => {
  try {
    serviceSchedules.value = await $fetch(`/api/Service/GetServiceSchedulesByOrder/${id}`)
  } catch (error) {
    alert('Błąd podczas ładowania serwisów')
  }
}

const getServiceStatusName = (status) => {
  const statuses = {
    0: 'Oczekujący',
    1: 'W trakcie',
    2: 'Zakończony',
    3: 'Anulowany',
    4: 'Wstrzymany',
    5: 'Oczekiwanie na części',
  }
  return statuses[status] || 'Nieznany status'
}

const loadOrderDetails = async () => {
  if (orderCode.value.trim()) {
    await fetchOrderDetails(orderCode.value.trim())
  } else {
    alert('Proszę wprowadzić kod zamówienia')
  }
}
</script>

<style scoped>
.service-schedule {
  margin-bottom: 20px;
  padding: 10px;
  border: 1px solid #ccc;
}
</style>
