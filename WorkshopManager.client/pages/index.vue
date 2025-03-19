<template>
    <v-card style="height: 100%; width: 100%; padding: 20px;">
      <div>
        <h1>Witamy w aplikacji WorkshopManager</h1>
        <p>Ta aplikacja pozwala na zarządzanie warsztatem samochodowym.</p>
      </div>
  
      <!-- Brakujące elementy magazynowe -->
      <v-container>
        <h2>Brakujące elementy magazynowe</h2>
        <Carousel v-bind="carouselConfig">
          <Slide v-for="item in missingInventoryItems" :key="item.id">
            <v-card class="inventory-card">
              <v-card-title>{{ item.name }}</v-card-title>
              <v-card-text>
                <p><strong>Ilość w magazynie:</strong> {{ item.quantityInStock }}</p>
                <p><strong>Ilość brakująca:</strong> {{ item.missingStock }}</p>
              </v-card-text>
            </v-card>
          </Slide>
          <template #addons>
            <Navigation />
            <Pagination />
          </template>
        </Carousel>
      </v-container>
  
      <!-- Kończące się elementy magazynowe -->
      <v-container>
        <h2>Kończące się elementy magazynowe</h2>
        <Carousel v-bind="carouselConfig">
          <Slide v-for="item in lowStockInventoryItems" :key="item.id">
            <v-card class="inventory-card">
              <v-card-title>{{ item.name }}</v-card-title>
              <v-card-text>
                <p><strong>Ilość w magazynie:</strong> {{ item.quantityInStock }}</p>
                <p><strong>Poziom zamówienia:</strong> {{ item.reorderLevel }}</p>
              </v-card-text>
            </v-card>
          </Slide>
          <template #addons>
            <Navigation />
            <Pagination />
          </template>
        </Carousel>
      </v-container>
    </v-card>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue'
  import 'vue3-carousel/dist/carousel.css'
  import { Carousel, Slide, Pagination, Navigation } from 'vue3-carousel'
  
  definePageMeta({
    middleware: 'auth',
    auth: {
      roles: ["Administrator", "Starszy Mechanik", "Młodszy Mechanik", "Klient"]
    },
    layout: 'default'
  })
  
  const missingInventoryItems = ref([])
  const lowStockInventoryItems = ref([])
  
  const fetchMissingInventoryItems = async () => {
    try {
      missingInventoryItems.value = await $fetch('/api/InventoryItem/GetMissingInventoryItems')
    } catch (error) {
      console.error('Błąd podczas ładowania brakujących elementów magazynu', error)
    }
  }
  
  const fetchLowStockInventoryItems = async () => {
    try {
      lowStockInventoryItems.value = await $fetch('/api/InventoryItem/GetLowStockInventoryItems')
    } catch (error) {
      console.error('Błąd podczas ładowania kończących się elementów magazynu', error)
    }
  }
  
  onMounted(() => {
    fetchMissingInventoryItems()
    fetchLowStockInventoryItems()
  })
  

  const carouselConfig = {
    itemsToShow: 4, 
    snapAlign: 'start', 
    wrapAround: true,
    autoplay: 4000,
    pauseAutoplayOnHover: true,
  }
  </script>
  
  <style scoped>
  h2 {
    text-align: center;
    margin-bottom: 10px;
  }
  
  .inventory-card {
    border: 2px solid #ccc; /* Ramka */
    text-align: center;
    padding: 10px;
    margin: 0 auto;
  }
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
  color: #000000;
}

.v-text-field {
  max-width: 300px;
  margin-left: auto;
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
  