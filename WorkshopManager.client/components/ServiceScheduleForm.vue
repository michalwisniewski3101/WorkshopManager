<template>
  <v-container>
    <v-btn color="primary" @click="openScheduleModal">Dodaj usługę serwisową</v-btn>

    <v-dialog v-model="showScheduleModal" max-width="600">
      <v-card>
        <v-card-title>Dodaj usługę serwisową</v-card-title>

        <v-card-text>
          <v-form @submit.prevent="addServiceSchedule">

            <!-- Rodzaj naprawy -->
            <v-select
              v-model="newServiceSchedule.serviceId"
              :items="services"
              item-title="serviceDescription"
              item-value="id"
              label="Rodzaj naprawy"
              required
            ></v-select>

            <!-- Mechanicy -->
            <div v-for="(mechanic, index) in newServiceSchedule.mechanics" :key="index" class="d-flex align-center mb-3">
              <v-select
                v-model="newServiceSchedule.mechanics[index]"
                :items="mechanics"
                item-title="firstName"
                item-value="id"
                :label="`Mechanik ${index + 1}`"
                required
                class="mr-3"
              ></v-select>
              <v-btn icon color="red" @click="removeMechanic(index)">
                <v-icon>mdi-delete</v-icon>
              </v-btn>
            </div>
            <v-btn text color="primary" @click="addMechanic">Dodaj kolejnego mechanika</v-btn>

            <!-- Pozycje zamówienia -->
            <div v-for="(item, index) in newServiceSchedule.orderItems" :key="index" class="mt-4">
              <v-select
                v-model="item.inventoryItemId"
                :items="inventoryItems"
                item-title="name"
                item-value="id"
                :label="`Część ${index + 1}`"
                required
              ></v-select>
              <v-text-field
                v-model.number="item.quantity"
                type="number"
                label="Ilość"
                min="1"
                class="mt-2"
                required
              ></v-text-field>
              <v-btn icon color="red" @click="removeOrderItem(index)">
                <v-icon>mdi-delete</v-icon>
              </v-btn>
            </div>
            <v-btn text color="primary" @click="addOrderItem">Dodaj kolejną część</v-btn>
          </v-form>
        </v-card-text>

        <v-card-actions>
          <v-btn color="primary" @click="addServiceSchedule">Dodaj usługę serwisową</v-btn>
          <v-btn color="grey" @click="openScheduleModal">Anuluj</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup>
import { ref, onMounted } from 'vue';

const emit = defineEmits(['service-schedule-added', 'show-snackbar']);

const props = defineProps({
  orderId: {
    type: String,
    required: true,
  },
});

// Definicja zmiennych
const services = ref([]);
const showScheduleModal = ref(false);
const mechanics = ref([]);
const inventoryItems = ref([]);
const newServiceSchedule = ref({
  serviceId: null,
  mechanics: [null],
  orderItems: [
    {
      inventoryItemId: null,
      quantity: 0,
    },
  ],
});

// Funkcja do ładowania danych
const fetchData = async () => {
  try {
    services.value = await $fetch('/api/Service');
    mechanics.value = await $fetch('/api/Mechanic/GetMechanicsList');
    inventoryItems.value = await $fetch('/api/InventoryItem');
  } catch (error) {
    console.error('Błąd podczas pobierania danych:', error);
  }
};

onMounted(async () => {
  await fetchData();
});

const addOrderItem = () => {
  newServiceSchedule.value.orderItems.push({
    inventoryItemId: null,
    quantity: 1,
  });
};

const removeOrderItem = (index) => {
  newServiceSchedule.value.orderItems.splice(index, 1);
};

const addMechanic = () => {
  newServiceSchedule.value.mechanics.push(null);
};

const removeMechanic = (index) => {
  newServiceSchedule.value.mechanics.splice(index, 1);
};

const openScheduleModal = () => {
  showScheduleModal.value = !showScheduleModal.value;
};

const addServiceSchedule = async () => {
  try {
    const payload = {
      serviceId: newServiceSchedule.value.serviceId,
      mechanics: newServiceSchedule.value.mechanics,
      orderItems: newServiceSchedule.value.orderItems.map(item => ({
        inventoryItemId: item.inventoryItemId,
        quantity: item.quantity,
      })),
    };

    await $fetch(`/api/Service/AddServiceSchedule/${props.orderId}`, {
      method: 'POST',
      body: JSON.stringify(payload),
      headers: { 'Content-Type': 'application/json' },
    });
    openScheduleModal();
    emit('service-schedule-added');
  } catch (error) {
    emit('show-snackbar', 'Błąd podczas dodawania harmonogramu.');
  }
};
</script>

<style scoped>
/* Możesz dodać niestandardowy styl tutaj */
</style>