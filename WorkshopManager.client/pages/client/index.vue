<template>
  <v-card>
  
    <h1>Szczegóły zamówienia</h1>
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
            <v-col cols="12">
              <span><strong>Kod Klienta:</strong> {{ order.clientCode }}</span>
            </v-col>
            <v-col cols="12" md="6">
              <span><strong>Szacowana data ukończenia:</strong>
                {{ order.estimatedCompletionDate ? new Date(order.estimatedCompletionDate).toLocaleDateString() : 'Brak' }}
              </span>
            </v-col>
            <v-col cols="12" md="6">
              <span><strong>Całkowity koszt:</strong> {{ order.totalCost ? `${order.totalCost} PLN` : 'Nie ustalono' }}</span>
            </v-col>
            <v-col cols="12" md="6">
              <Payment />
            </v-col>
          </v-row>
        </v-card-title>
      </v-card>
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
        <!-- Okno do podawania kodu zamówienia -->
        <div v-else>
      <label for="orderCode">Wprowadź kod zamówienia:</label>
      <input v-model="orderCode" id="orderCode" type="text" placeholder="Wprowadź kod" />
      <button @click="loadOrderDetails">Pokaż zamówienie</button>
    </div>
</v-card>
</template>

<script setup>
  definePageMeta({
  layout: 'login'
})



import { ref } from 'vue'
import { useRoute } from 'vue-router'

const order = ref(null)
const serviceSchedules = ref([])
const services = ref([])
const mechanics = ref([])
const inventoryItems = ref([])

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

const fetchOrderDetails = async (code) => {
  try {
    order.value = await $fetch(`/api/Order/ByCode/${code}`)
    if (order.value && order.value.id) {
      await fetchServiceSchedules(order.value.id)
      await fetchAdditionalData()
    }
  } catch (error) {
    alert('Błąd podczas ładowania szczegółów zamówienia')
  }
}

const fetchServiceSchedules = async (id) => {
  try {
    const schedules = await $fetch(`/api/Service/GetServiceSchedulesByOrder/${id}`)
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
    return mechanic ? `${mechanic.firstName} ${mechanic.lastName.charAt(0)}.` : 'Nieznany mechanik';

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
const loadOrderDetails = async () => {
  if (orderCode.value.trim()) {
    await fetchOrderDetails(orderCode.value.trim())
  } else {
    alert('Proszę wprowadzić kod zamówienia')
  }
}

</script>

<style scoped>

</style>
