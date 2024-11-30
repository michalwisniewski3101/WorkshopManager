<template>
  <div>
    <button @click="openScheduleModal">Dodaj usługę serwisową</button>


    <div v-if="showScheduleModal" class="modal">
      <form @submit.prevent="addServiceSchedule">

        <div>
          <label for="service">Rodzaj naprawy:</label>
          <select v-model="newServiceSchedule.service" id="service" required>
            <option disabled value="">Wybierz specjalność</option>
            <option v-for="service in services" :key="service.id" :value="service.id">
              {{ service.serviceDescription }}
            </option>
          </select>
        </div>

        <!-- Mechanics -->
        <div>
          <label for="mechanic">Mechanik:</label>
          <select v-model="newServiceSchedule.mechanics" id="mechanic" multiple required>
            <option v-for="mechanic in mechanics" :key="mechanic.id" :value="mechanic.id">
              {{ mechanic.firstName }}
            </option>
          </select>
        </div>

        <!-- Order Items -->
        <select v-model="newServiceSchedule.orderItems" :items="orderItems" item-value="inventoryItemId"
          item-text="name" label="Wybierz przedmioty zamówienia" multiple></select>

        <!-- Service Status -->
        <select v-model="newServiceSchedule.serviceStatus" :items="serviceStatuses" label="Status usługi"
          required></select>
        <button type="submit">Dodaj usługę serwisowa</button>
        <button @click="openScheduleModal" type="button">Anuluj</button>
      </form>



    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useFetch } from '#app'; // Zamiast axios, jeśli chcesz używać Nuxt 3 fetch

// Definicja zmiennych
const dialog = ref(false);
const valid = ref(false);
const services = ref([]);
const showScheduleModal = ref(false)
const mechanics = ref([]);
const orderItems = ref([]);
const serviceStatuses = ref([0, 1, 2]); // Statusy: 0 - oczekujące, 1 - w trakcie, 2 - zakończona
const newServiceSchedule = ref({
  service: null,
  mechanics: [],
  serviceStatus: null,
  orderItems: [],
});

// Funkcja do ładowania danych
const fetchData = async () => {
  try {
    services.value = await $fetch('/api/Service');
     

    mechanics.value = await $fetch('/api/Mechanic/GetMechanicsList');
     

    orderItems.value  = await $fetch('/api/InventoryItem');
    
  } catch (error) {
    console.error('Błąd podczas pobierania danych:', error);
  }
};

// Ładowanie danych po zamontowaniu komponentu
onMounted(async () => {
  await fetchData();
});

const openScheduleModal = () => {
  showScheduleModal.value = !showScheduleModal.value
}

// Funkcja dodająca harmonogram usługi
const addServiceSchedule = async () => {
  try {
    const payload = {
      serviceDateStart: newServiceSchedule.value.serviceDateStart,
      serviceDateEnd: newServiceSchedule.value.serviceDateEnd,
      service: newServiceSchedule.value.service,
      mechanics: newServiceSchedule.value.mechanics,
      serviceStatus: newServiceSchedule.value.serviceStatus,
      orderItems: newServiceSchedule.value.orderItems.map(item => ({
        inventoryItemId: item,
        quantity: 1, // Przykładowa ilość, dostosuj według potrzeb
        totalPrice: 0, // Adjust accordingly
      })),
    };

    await $fetch('/api/serviceSchedules', {
      method: 'POST',
      body: JSON.stringify(payload),
      headers: { 'Content-Type': 'application/json' },
    });

    // Emitowanie zdarzenia do rodzica
    emit('service-schedule-added', payload);
    dialog.value = false;
    emit('show-snackbar', 'Harmonogram usługi został dodany pomyślnie!');
  } catch (error) {
    emit('show-snackbar', 'Błąd podczas dodawania harmonogramu.');
  }
};
</script>

<style scoped>
/* Możesz dodać niestandardowy styl tutaj */
</style>
