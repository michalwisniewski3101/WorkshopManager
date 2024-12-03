<template>
  <v-container>
    <h1 class="text-center mb-4">Szczegóły zamówienia</h1>

    <div v-if="order">
      <v-card class="mb-4">
        <v-card-title>
          <v-row>
            <v-col cols="12" md="6">
              <span><strong>ID:</strong> {{ order.id }}</span>
            </v-col>
            <v-col cols="12" md="6">
              <span><strong>Data zamówienia:</strong> {{ new Date(order.orderDate).toLocaleDateString() }}</span>
            </v-col>
            <v-col cols="12" md="6">
              <span><strong>Klient:</strong> {{ order.clientName }}</span>
            </v-col>
            <v-col cols="12" md="6">
              <span><strong>Telefon:</strong> {{ order.clientPhoneNumber }}</span>
            </v-col>
            <v-col cols="12">
              <span><strong>Opis:</strong> {{ order.description }}</span>
            </v-col>
            <v-col cols="12">
              <span><strong>Status:</strong> {{ getOrderStatusName(order.orderStatus) }}</span>
            </v-col>
            <v-col cols="12" md="6">
              <span><strong>Szacowana data ukończenia:</strong>
                {{ order.estimatedCompletionDate ? new Date(order.estimatedCompletionDate).toLocaleDateString() : 'Brak' }}
              </span>
            </v-col>
            <v-col cols="12" md="6">
              <span><strong>Całkowity koszt:</strong> {{ order.totalCost ? `${order.totalCost} PLN` : 'Nie ustalono' }}</span>
            </v-col>
          </v-row>
        </v-card-title>
      </v-card>

      <service-schedule-form @service-schedule-added="fetchServiceSchedules" :orderId="order.id" />

      <v-divider class="my-4"></v-divider>

      <div v-if="serviceSchedules && serviceSchedules.length > 0">
        <h2 class="text-center mb-4">Harmonogramy serwisów</h2>
        <v-card v-for="schedule in serviceSchedules" :key="schedule.id" class="service-schedule mb-4">
          <v-card-title>
            <v-row>
              <v-col cols="12" md="6">
                <span><strong>Harmonogram ID:</strong> {{ schedule.id }}</span>
              </v-col>
              <v-col cols="12" md="6">
                <span><strong>Data rozpoczęcia:</strong> {{ schedule.serviceDateStart ? new Date(schedule.serviceDateStart).toLocaleDateString() : 'Brak' }}</span>
              </v-col>
              <v-col cols="12" md="6">
                <span><strong>Data zakończenia:</strong> {{ schedule.serviceDateEnd ? new Date(schedule.serviceDateEnd).toLocaleDateString() : 'Brak' }}</span>
              </v-col>
              <v-col cols="12" md="6">
                <span><strong>Serwis ID:</strong> {{ schedule.serviceId }}</span>
              </v-col>
              <v-col cols="12">
                <span><strong>Mechanicy:</strong> {{ schedule.mechanics.join(', ') }}</span>
              </v-col>
            </v-row>
          </v-card-title>

          <v-card-actions>
            <div v-if="schedule.showStatusChange">
              <v-select v-model="schedule.selectedStatus" :items="statusOptions" :item-title="'name'" :item-value="'value'" label="Wybierz status" class="mr-4" />
              <v-btn @click="updateServiceScheduleStatus(schedule.id)" color="success">Zapisz zmiany</v-btn>
              <v-btn @click="schedule.showStatusChange = false" color="error">Anuluj</v-btn>
            </div>
            <div v-if="!schedule.showStatusChange">

              <span>{{ getServiceStatusName(schedule.serviceStatus) }}</span>
              <v-btn @click="changeServiceStatus(schedule.id)" color="primary">
                <v-icon>mdi-pencil</v-icon>
                Zmień status
              </v-btn>
              
            </div>


          </v-card-actions>

          <v-divider class="my-4"></v-divider>

          <v-card-subtitle>
            <h3>Pozycje zamówienia:</h3>
            <v-list>
              <v-list-item v-for="item in schedule.orderItems" :key="item.inventoryItemId">
                <v-list-item-content>
                  <v-list-item-title><strong>Pozycja ID:</strong> {{ item.inventoryItemId }}</v-list-item-title>
                  <v-list-item-subtitle>
                    <span><strong>Ilość:</strong> {{ item.quantity }}</span>
                    <span><strong>Całkowita cena:</strong> {{ item.totalPrice }} PLN</span>
                  </v-list-item-subtitle>
                </v-list-item-content>
              </v-list-item>
            </v-list>
          </v-card-subtitle>
        </v-card>
      </div>
    </div>

    <NuxtLink to="/order">
      <v-btn color="secondary" class="mt-4">Wróć do listy zamówień</v-btn>
    </NuxtLink>
  </v-container>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'

const order = ref(null)
const serviceSchedules = ref([])

const route = useRoute()

const statusOptions = [
  { name: 'Oczekujący', value: 0 },
  { name: 'Oczekiwanie na części', value: 1 },
  { name: 'W trakcie', value: 2 },
  { name: 'Zakończony', value: 3 },
  { name: 'Anulowany', value: 4 }
]

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
/* Można dodać własne style, jeśli wymagane */
</style>
