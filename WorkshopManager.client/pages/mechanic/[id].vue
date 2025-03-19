<template>
  <v-card style="height: 100%; width: 100%;">

    <v-breadcrumbs :items="items">
      </v-breadcrumbs>
      <v-row>
      <v-col cols="12" class="text-left">
        <h1>Historia zamówień pracownika</h1>
      </v-col>
      <v-col cols="12" class="text-right">
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


    <div v-if="order">
      <v-card class="mb-4">
        <v-card-title>
          <v-row>
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

        <v-expansion-panels>
          <v-expansion-panel v-for="schedule in serviceSchedules" :key="schedule.id">
            <v-expansion-panel-title>
            <v-row>
              <v-col cols="12" md="6">
                <span>{{ getServiceDescription(schedule.serviceId) }}</span>

              </v-col>
              <v-col cols="12" md="6">
                <span>{{ getServiceStatusName(schedule.serviceStatus) }}</span>
              </v-col>
            </v-row>
          </v-expansion-panel-title>
          <v-expansion-panel-text>
         
            <div v-if="schedule.showStatusChange">
              <v-select v-model="schedule.selectedStatus" :items="statusOptions" :item-title="'name'" :item-value="'value'" label="Wybierz status" class="mr-4" />
              <v-btn @click="updateServiceScheduleStatus(schedule.id)" color="success">Zapisz zmiany</v-btn>
              <v-btn @click="schedule.showStatusChange = false" color="error">Anuluj</v-btn>
            </div>
            <div v-if="!schedule.showStatusChange">
              
              <v-btn @click="changeServiceStatus(schedule.id)" color="primary">
                <v-icon>mdi-pencil</v-icon>
                Zmień status
              </v-btn>
            </div>
            <span><strong>Mechanicy:</strong>
                  {{ getMechanicsNames(schedule.mechanics) }}
                </span>
          <v-divider class="my-4"></v-divider>

          
            <h3>Pozycje zamówienia:</h3>
            <v-list>
              <v-list-item v-for="item in schedule.orderItems" :key="item.inventoryItemId">
                <v-list-item-content>
                  <v-list-item-title>
                    {{ getInventoryItemName(item.inventoryItemId) }}
                  </v-list-item-title>
                  <v-list-item-subtitle>
                    <span><strong>Ilość:</strong> {{ item.quantity }} </span>
                    <span><strong> Całkowita cena:</strong> {{ item.totalPrice }} PLN</span>
                  </v-list-item-subtitle>
                </v-list-item-content>
              </v-list-item>
            </v-list>
          
          </v-expansion-panel-text>
        </v-expansion-panel>
        </v-expansion-panels>
      </div>
    </div>
  </v-card>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'

const orders = ref([])
const serviceSchedules = ref([])
const services = ref([])
const mechanics = ref([])
const inventoryItems = ref([])
const search = ref("")
const items = ref([
  {
    title: 'Dashboard',
    disabled: false,
    to: '/',
  },
  {
    title: 'Pracownicy',
    disabled: false,
    to: 'mechanic-list',
  },
  {
    title: 'Zamówienia',
    disabled: true,
    to: '',
  },
]);
const route = useRoute()
const tableHeaders = [
  { title: 'Zamówienie ID', key: 'id' },
  { title: 'Data zamówienia', key: 'orderDate' },
  { title: 'Klient', key: 'clientName' },
  { title: 'Status', key: 'status' },
  { title: 'Akcje', key: 'actions', sortable: false }
]
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

const fetchAdditionalData = async () => {
  try {
    services.value = await $fetch('/api/Service');
    mechanics.value = await $fetch('/api/Mechanic/GetMechanicsList');
    inventoryItems.value = await $fetch('/api/InventoryItem');
  } catch (error) {
    alert('Błąd podczas ładowania danych dodatkowych');
  }
};

const getServiceDescription = (serviceId) => {
  const service = services.value.find(s => s.id === serviceId);
  return service ? service.serviceDescription : 'Nieznana usługa';
}

const getMechanicsNames = (mechanicIds) => {
  return mechanicIds.map(id => {
    const mechanic = mechanics.value.find(m => m.id === id);
    return mechanic ? `${mechanic.firstName} ${mechanic.lastName}` : 'Nieznany mechanik';
  }).join(', ');
}

const getInventoryItemName = (itemId) => {
  const item = inventoryItems.value.find(i => i.id === itemId);
  return item ? item.name : 'Nieznana pozycja';
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
      body: JSON.stringify(schedule.selectedStatus),
    })
    schedule.showStatusChange = false
    await fetchServiceSchedules()
    await fetchOrderDetails()
  } catch (error) {
    alert('Błąd podczas aktualizacji statusu')
  }
}

const fetchOrdersByWorker = async () => {
  try {
    orders.value = await $fetch(`/api/Order/ByWorker/${route.params.id}`)
  } catch (error) {
    alert('Błąd podczas ładowania listy zamówień')
  }
}
onMounted(fetchOrdersByWorker)
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
</style>
