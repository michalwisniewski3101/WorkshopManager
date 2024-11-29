<template>
  <div>
    <button @click="openMechanicModal">Dodaj nowego mechanika</button>

        
        <div v-if="showMechanicModal" class="modal">
          <v-form ref="form" v-model="valid" @submit.prevent="addServiceSchedule">
            <!-- Service Date Start -->
            <v-text-field
              v-model="newServiceSchedule.serviceDateStart"
              label="Data rozpoczęcia usługi"
              type="datetime-local"
              required
            ></v-text-field>

            <!-- Service Date End -->
            <v-text-field
              v-model="newServiceSchedule.serviceDateEnd"
              label="Data zakończenia usługi"
              type="datetime-local"
              required
            ></v-text-field>

            <!-- Service -->
            <v-select
              v-model="newServiceSchedule.service"
              :items="services"
              item-value="id"
              item-text="serviceDescription"
              label="Wybierz usługę"
              required
            ></v-select>

            <!-- Mechanics -->
            <v-select
              v-model="newServiceSchedule.mechanics"
              :items="mechanics"
              item-value="id"
              item-text="name"
              label="Wybierz mechaników"
              multiple
              required
            ></v-select>

            <!-- Order Items -->
            <v-select
              v-model="newServiceSchedule.orderItems"
              :items="orderItems"
              item-value="inventoryItemId"
              item-text="name"
              label="Wybierz przedmioty zamówienia"
              multiple
            ></v-select>

            <!-- Service Status -->
            <v-select
              v-model="newServiceSchedule.serviceStatus"
              :items="serviceStatuses"
              label="Status usługi"
              required
            ></v-select>
            <button type="submit">Dodaj Mechanika</button>
            <button @click="closeMechanicModal" type="button">Anuluj</button>
          </v-form>
        

     
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
const showMechanicModal = ref(false)
const mechanics = ref([]);
const orderItems = ref([]);
const serviceStatuses = ref([0, 1, 2]); // Statusy: 0 - oczekujące, 1 - w trakcie, 2 - zakończona
const newServiceSchedule = ref({
  serviceDateStart: '',
  serviceDateEnd: '',
  service: null,
  mechanics: [],
  serviceStatus: null,
  orderItems: [],
});

// Funkcja do ładowania danych
const fetchData = async () => {
  try {
    const { data: servicesData } = await useFetch('/api/Service');
    services.value = servicesData;

    const { data: mechanicsData } = await useFetch('/api/Mechanic/GetMechanicsList');
    mechanics.value = mechanicsData;

    const { data: orderItemsData } = await useFetch('/api/InventoryItem');
    orderItems.value = orderItemsData;
  } catch (error) {
    console.error('Błąd podczas pobierania danych:', error);
  }
};

// Ładowanie danych po zamontowaniu komponentu
onMounted(() => {
  fetchData();
});

// Funkcja do otwarcia dialogu
const openDialog = () => {
  dialog.value = true;
};

// Funkcja dodająca harmonogram usługi
const addServiceSchedule = async () => {
  try {
    const payload = {
      id: new Date().toISOString(), // Możesz użyć GUID, jeśli potrzebujesz unikalnego identyfikatora
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
