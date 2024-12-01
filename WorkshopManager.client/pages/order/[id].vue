<template>
  <div>
    <h1>Szczegóły zamówienia</h1>
    <div v-if="order">
      <p><strong>ID:</strong> {{ order.id }}</p>
      <p><strong>Data zamówienia:</strong> {{ new Date(order.orderDate).toLocaleDateString() }}</p>
      <p><strong>Klient:</strong> {{ order.clientName }}</p>
      <p><strong>Telefon:</strong> {{ order.clientPhoneNumber }}</p>
      <p><strong>Opis:</strong> {{ order.description }}</p>
      <p><strong>Status:</strong>
        {{ getOrderStatusName(order.orderStatus) }}
      </p>
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

      <service-schedule-form @service-schedule-added="fetchServiceSchedules" :orderId="order.id" />

      <div v-if="serviceSchedules && serviceSchedules.length > 0">
        <h2>Harmonogramy serwisów</h2>
        <div v-for="schedule in serviceSchedules" :key="schedule.id" class="service-schedule">
          <p><strong>Harmonogram ID:</strong> {{ schedule.id }}</p>
          <p><strong>Data rozpoczęcia:</strong> {{ schedule.serviceDateStart ? new
            Date(schedule.serviceDateStart).toLocaleDateString() : 'Brak' }}</p>
          <p><strong>Data zakończenia:</strong> {{ schedule.serviceDateEnd ? new
            Date(schedule.serviceDateEnd).toLocaleDateString() : 'Brak' }}</p>
          <p><strong>Serwis ID:</strong> {{ schedule.serviceId }}</p>
          <p><strong>Mechanicy:</strong> {{ schedule.mechanics.join(', ') }}</p>
          <p><strong>Status serwisu:</strong>
            <div v-if="!schedule.showStatusChange">
              {{ getServiceStatusName(schedule.serviceStatus) }}
              <button @click="changeServiceStatus(schedule.id)">Zmień status</button>
            </div>

            <div v-if="schedule.showStatusChange">
              <select v-model="schedule.selectedStatus">
                <option v-for="(status, value) in statusOptions" :key="value" :value="value">{{ status }}</option>
              </select>
              <button @click="updateServiceScheduleStatus(schedule.id)">Zapisz zmiany</button>
              <button @click="schedule.showStatusChange = false">Anuluj</button>
            </div>
          </p>
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

    <NuxtLink to="/order"><button>Wróć do listy zamówień</button></NuxtLink>


  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'

const order = ref(null)
const serviceSchedules = ref([])
const route = useRoute()

const statusOptions = {
    0: 'Oczekujący',
    1: 'Oczekiwanie na części',
    2: 'W trakcie',
    3: 'Zakończony',
    4: 'Anulowany'
}

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

const fetchOrderDetails = async () => {
  try {
    order.value = await $fetch(`/api/Order/${route.params.id}`)
  } catch (error) {
    alert('Błąd podczas ładowania szczegółów zamówienia')
  }
}

const fetchServiceSchedules = async () => {
  try {
    const schedules = await $fetch(`/api/Service/GetServiceSchedulesByOrder/${route.params.id}`)
    serviceSchedules.value = schedules.map(schedule => ({
      ...schedule,
      showStatusChange: false, 
      selectedStatus: schedule.serviceStatus 
    }))
  } catch (error) {
    alert('Błąd podczas ładowania serwisów')
  }
}

const getServiceStatusName = (status) => {
  const statuses = {
    0: 'Oczekujący',
    1: 'Oczekiwanie na części',
    2: 'W trakcie',
    3: 'Zakończony',
    4: 'Anulowany'
  }
  return statuses[status] || 'Nieznany status'
}

const changeServiceStatus = (scheduleId) => {
  const schedule = serviceSchedules.value.find(s => s.id === scheduleId)
  if (schedule) schedule.showStatusChange = true
}

const updateServiceScheduleStatus = async (scheduleId) => {
  const schedule = serviceSchedules.value.find(s => s.id === scheduleId)
  if (!schedule) return

  try {
    await $fetch(`/api/Service/UpdateServiceScheduleStatus/${scheduleId}`, {
      method: 'PUT',
      body: schedule.selectedStatus,
    })
    schedule.showStatusChange = false
    await fetchServiceSchedules()
    await fetchOrderDetails()
  } catch (error) {
    alert('Błąd podczas aktualizacji statusu')
  }
}

onMounted(async () => {
  await fetchOrderDetails()
  await fetchServiceSchedules()
})
</script>

<style scoped>
.service-schedule {
  margin-bottom: 20px;
  padding: 10px;
  border: 1px solid #ccc;
}

.status-change-form {
  margin-top: 20px;
  padding: 10px;
  border: 1px solid #ccc;
  background-color: #f9f9f9;
}
</style>
