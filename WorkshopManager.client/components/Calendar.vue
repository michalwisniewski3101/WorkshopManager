<template>
    <v-card style="height: 100%; width: 100%; padding: 20px;">
      <h2>Harmonogram zamówień</h2>
      
      <vue-cal 
        :events="orderEvents" 
        locale="pl" 
        events-on-month-view
        style="height: 500px"
        @event-click="showOrderDetails"
      />
  
      <v-dialog v-model="dialog" max-width="500">
        <v-card>
          <v-card-title>{{ selectedOrder?.title }}</v-card-title>
          <v-card-text>
            <p><strong>Data rozpoczęcia:</strong> {{ formatDate(selectedOrder?.start) }}</p>
            <p><strong>Data zakończenia:</strong> {{ selectedOrder?.end ? formatDate(selectedOrder?.end) : 'Brak danych' }}</p>
            <p><strong>Status:</strong> {{ getOrderStatus(selectedOrder?.orderStatus) }}</p>
          </v-card-text>
          <v-card-actions>
            <v-btn style="background-color: #f15c5c;" @click="dialog = false">Zamknij</v-btn>
            <v-btn style="background-color: #4caf50;" :to="`/order/${selectedOrder?.id}`">Szczegóły</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </v-card>
  </template>
  
  <script setup>
  import VueCal from 'vue-cal';
  import 'vue-cal/dist/vuecal.css';
  import { ref, onMounted } from 'vue';
  
  const orderEvents = ref([]);
  const dialog = ref(false);
  const selectedOrder = ref(null);
  
  const fetchOrderData = async () => {
  try {
    const orders = await $fetch('/api/Order');
    orderEvents.value = orders.map(order => ({
      id: order.id,
      title: order.description,
      start: formatDate(order.orderDate),
      end: order.estimatedCompletionDate 
        ? formatDate(order.estimatedCompletionDate)
        : formatDate(new Date(new Date(order.orderDate).getTime() + 2 * 60 * 60 * 1000)),
      orderStatus: order.orderStatus,
      color: getOrderColor(order.orderStatus),
    }));
    console.log('Załadowano dane zamówień', orderEvents.value);
  } catch (error) {
    console.error('Błąd podczas ładowania danych zamówień', error);
  }
};
  
  const showOrderDetails = (event) => {
    selectedOrder.value = event;
    dialog.value = true;
  };
  
  const formatDate = (date) => {
    if (!date) return '';
    const d = new Date(date);
    return d.toISOString().slice(0, 19).replace('T', ' ');
  };
  
  const getOrderColor = (status) => {
    const statusColors = {
      1: "blue",    
      2: "orange",  
      3: "green",   
    };
    return statusColors[status] || "grey";
  };
  
  const getOrderStatus = (status) => {
    const statusMap = {
      1: 'Nowe',
      2: 'W trakcie',
      3: 'Zakończone',
    };
    return statusMap[status] || 'Nieznany';
  };
  
  onMounted(() => {
    fetchOrderData();
  });
  </script>
  
  <style scoped>
  h2 {
    text-align: center;
    margin-bottom: 10px;
  }
  
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
  