<template>
  <v-container>
    <v-btn style="background-color: #4caf50;" @click="openScheduleModal">Dodaj usługę serwisową</v-btn>

    <v-dialog v-model="showScheduleModal" max-width="800">
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

            <div v-for="(mechanic, index) in newServiceSchedule.mechanics" :key="index" class="d-flex align-center mb-3">
             <v-row>
              <v-col cols="11">
                <v-select
                v-model="newServiceSchedule.mechanics[index]"
                :items="mechanics"
                item-title="firstName"
                item-value="id"
                :label="`Mechanik ${index + 1}`"
                required
              ></v-select>
              </v-col>
              
              <v-col cols="1">
                <v-btn icon variant="text" @click="removeMechanic(index)">
  <v-icon color="black">mdi-delete</v-icon>
</v-btn>

              </v-col>
             </v-row>
            </div>
            <v-btn text style="background-color: #4caf50;" @click="addMechanic">Dodaj kolejnego mechanika</v-btn>

            <!-- Pozycje zamówienia -->
            <div v-for="(item, index) in newServiceSchedule.orderItems" :key="index" class="mt-4">
              <v-row>
                <v-col cols="9">
                  <v-select
                v-model="item.inventoryItemId"
                :items="inventoryItems"
                item-title="name"
                item-value="id"
                :label="`Część ${index + 1}`"
                required
              ></v-select></v-col>
              <v-col cols="2">
                <v-text-field
                v-model.number="item.quantity"
                type="number"
                label="Ilość"
                min="1"
                required
              ></v-text-field>
              

                </v-col>
                <v-col cols="1">
                <v-btn icon variant="text" @click="removeOrderItem(index)">
  <v-icon color="black">mdi-delete</v-icon>
</v-btn>
                </v-col>
              </v-row>



            </div>
            <v-btn text style="background-color: #4caf50;" @click="addOrderItem">Dodaj kolejną część</v-btn>
          </v-form>
        </v-card-text>

        <v-card-actions>
          <v-btn style="background-color: #f15c5c; "@click="openScheduleModal">Anuluj</v-btn>
          <v-btn style="background-color: #4caf50; " @click="addServiceSchedule">Dodaj usługę serwisową</v-btn>
          
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
  .v-card {
    padding: 20px;
    background: #e0e0e0;
    border-radius: 10px;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1) !important;
    color: #000000;
  }
  
  .v-dialog .v-card {
    padding: 15px;
  }
</style>