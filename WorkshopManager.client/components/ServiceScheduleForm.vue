<template>
  <div>
    <button @click="openScheduleModal">Dodaj usługę serwisową</button>

    <div v-if="showScheduleModal" class="modal">
      <form @submit.prevent="addServiceSchedule">

        <div>
          <label for="service">Rodzaj naprawy:</label>
          <select v-model="newServiceSchedule.serviceId" id="service" required>
            <option disabled value="">Wybierz specjalność</option>
            <option v-for="service in services" :key="service.id" :value="service.id">
              {{ service.serviceDescription }}
            </option>
          </select>
        </div>

<!-- Mechanics -->
<div v-for="(mechanic, index) in newServiceSchedule.mechanics" :key="index">
          <label :for="'mechanic-' + index">Mechanik:</label>
          <select v-model="newServiceSchedule.mechanics[index]" :id="'mechanic-' + index" required>
            <option disabled value="">Wybierz mechanika</option>
            <option v-for="mechanicOption in mechanics" :key="mechanicOption.id" :value="mechanicOption.id">
              {{ mechanicOption.firstName }}
            </option>
          </select>
          <button type="button" @click="removeMechanic(index)">Usuń mechanika</button>
        </div>

        <button type="button" @click="addMechanic">Dodaj kolejnego mechanika</button>

        <!-- Order Items -->
        <div v-for="(item, index) in newServiceSchedule.orderItems" :key="index">
          <div>
            <label :for="'inventoryItemId-' + index">Część:</label>
            <select v-model="item.inventoryItemId" :id="'inventoryItemId-' + index" required>
              <option disabled value="">Wybierz część</option>
              <option v-for="inventoryItem in inventoryItems" :key="inventoryItem.id" :value="inventoryItem.id">
                {{ inventoryItem.name }}
              </option>
            </select>
          </div>

          <div>
            <label :for="'quantity-' + index">Ilość:</label>
            <input type="number" v-model="item.quantity" :id="'quantity-' + index" min="1" required />
            <button type="button" @click="removeOrderItem(index)">Usuń część</button>
          </div>

          <button type="button" @click="addOrderItem">Dodaj kolejną część</button>
        </div>

        

        <button type="submit">Dodaj usługę serwisową</button>
        <button @click="openScheduleModal" type="button">Anuluj</button>
      </form>
    </div>
  </div>
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
const showScheduleModal = ref(false)
const mechanics = ref([]);
const inventoryItems = ref([]);
const newServiceSchedule = ref({
  serviceId: null,
  mechanics: [null],
  orderItems: [
  {
      inventoryItemId: null,
      quantity: 0,                   
    }
  ],
});

// Funkcja do ładowania danych
const fetchData = async () => {
  try {
    services.value = await $fetch('/api/Service');
     

    mechanics.value = await $fetch('/api/Mechanic/GetMechanicsList');
     

    inventoryItems.value  = await $fetch('/api/InventoryItem');
    
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
  newServiceSchedule.value.mechanics.push(null); // Dodanie pustego mechanika
};

const removeMechanic = (index) => {
  newServiceSchedule.value.mechanics.splice(index, 1); // Usunięcie mechanika z listy
};

const openScheduleModal = () => {
  showScheduleModal.value = !showScheduleModal.value
}

// Funkcja dodająca harmonogram usługi
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
