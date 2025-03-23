<template>
  <v-card style="height: 100%; width: 100%;">
    <v-breadcrumbs :items="items">
      </v-breadcrumbs>
      <v-row>
        <v-col cols="12" class="text-left">
          <h1>Lista słowników</h1>
        </v-col>
        <v-col v-if="tab === 'specialties'" cols="6" class="text-left">
          <v-btn  @click="showSpecialtyModal = true" style="background-color: #4caf50;">
            <v-icon>mdi-plus</v-icon>Dodaj Specjalność
      </v-btn>
      </v-col>
      <v-col v-if="tab === 'services'" cols="6" class="text-left">
        <v-btn  @click="showServiceModal = true" style="background-color: #4caf50;">
          <v-icon>mdi-plus</v-icon>Dodaj Serwis</v-btn>
      </v-col>
      <v-col  cols="6" class="text-right">
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


      <v-row>
        <v-col cols="12">
    <v-tabs v-model="tab" bg-color="rgb(185, 185, 185)">
      <v-tab value="specialties">Specjalności</v-tab>
      <v-tab value="services">Serwisy</v-tab>
    </v-tabs>

   
      <v-tabs-window v-model="tab">
        <!-- Tabela Specjalności -->
        <v-tabs-window-item value="specialties">
          <v-data-table
            :headers="specialtyHeaders"
            :items="specialties"
            item-value="id"
            item-key="id"
            :search="search"
          >
            <template v-slot:[`item.specialtyName`]="{ item }">
              <span>{{ item.specialtyName }}</span>
            </template>
            <template v-slot:item.actions="{ item }">
        <v-icon @click="editItem(item)" class="clickable-icon">mdi-pencil</v-icon>
        <v-icon @click="deleteItem(item.id)" class="clickable-icon">mdi-delete</v-icon>
      </template>
          </v-data-table>
         
        

<!-- Modal dodawania specjalności -->
<v-dialog v-model="showSpecialtyModal" persistent max-width="400px">
  <v-card>
    <v-card-title class="headline">Dodaj Specjalność</v-card-title>
    <v-card-text>
      <form @submit.prevent="addSpecialty">
        <v-text-field
          label="Nazwa specjalności"
          v-model="newSpecialty.specialtyName"
          required
        ></v-text-field>
      </form>
    </v-card-text>
    <v-card-actions>
      <v-btn @click="closeSpecialtyModal" style="background-color: #f15c5c;">Anuluj</v-btn>
      <v-btn @click="addSpecialty" style="background-color: #4caf50;">Dodaj</v-btn>
    </v-card-actions>
  </v-card>
</v-dialog>
        </v-tabs-window-item>

        <!-- Tabela Serwisów -->
        <v-tabs-window-item value="services">
          <v-data-table
            :headers="serviceHeaders"
            :items="services"
            item-value="id"
            item-key="id"
            :search="search"
          >
            <template v-slot:[`item.serviceDescription`]="{ item }">
              <span>{{ item.serviceDescription }}</span>
            </template>
            <template v-slot:[`item.serviceCost`]="{ item }">
              <span>{{ item.serviceCost.toFixed(2) }} PLN</span>
            </template>
            <template v-slot:[`item.serviceDuration`]="{ item }">
              <span>{{ item.serviceDuration }} min</span>
            </template>
            <template v-slot:item.actions="{ item }">
        <v-icon @click="editItem(item)" class="clickable-icon">mdi-pencil</v-icon>
        <v-icon @click="deleteItem(item.id)" class="clickable-icon">mdi-delete</v-icon>
      </template>
          </v-data-table>
         
        

<!-- Modal dodawania serwisu -->
<v-dialog v-model="showServiceModal" persistent max-width="400px">
  <v-card>
    <v-card-title class="headline">Dodaj Serwis</v-card-title>
    <v-card-text>
      <form @submit.prevent="addService">
        <v-text-field
          label="Opis serwisu"
          v-model="newService.serviceDescription"
          required
        ></v-text-field>
        <v-text-field
          label="Koszt serwisu (PLN)"
          v-model.number="newService.serviceCost"
          type="number"
          step="0.01"
          required
        ></v-text-field>
        <v-text-field
          label="Czas trwania serwisu (minuty)"
          v-model.number="newService.serviceDuration"
          type="number"
          required
        ></v-text-field>
      </form>
    </v-card-text>
    <v-card-actions>
      
      <v-btn @click="closeServiceModal" style="background-color: #f15c5c;">Anuluj</v-btn>
      <v-btn @click="addService" style="background-color: #4caf50;">Dodaj</v-btn>
      
    </v-card-actions>
  </v-card>
</v-dialog>
        </v-tabs-window-item>
      </v-tabs-window>
    </v-col>
  </v-row>
  </v-card>
</template>

<script setup>
import { ref, onMounted } from 'vue'


definePageMeta({
  middleware: 'auth',
  auth: {
    roles: ['Administrator', 'Starszy Mechanik']
  }
})
const search = ref("")
const showSpecialtyModal = ref(false)
const showServiceModal = ref(false)
const items = ref([
  {
    title: 'Dashboard',
    disabled: false,
    to: '/',
  },
  {
    title: 'Słowniki',
    disabled: true,
    to: 'dictionaries',
  },
]);

const specialties = ref([])
const newSpecialty = ref({ specialtyName: '' })
const tab = ref("specialties")
const specialtyHeaders = ref([
  { title: 'Specjalność', align: 'start', key: 'specialtyName' },
  { title: 'Akcje', key: 'actions', sortable: false }
])


const services = ref([])
const newService = ref({
  serviceDescription: '',
  serviceCost: 0,
  serviceDuration: 0
})
const serviceHeaders = ref([
  { title: 'Opis Serwisu', align: 'start', key: 'serviceDescription' },
  { title: 'Koszt (PLN)', align: 'start', key: 'serviceCost' },
  { title: 'Czas trwania (min)', align: 'start', key: 'serviceDuration' },
  { title: 'Akcje', key: 'actions', sortable: false }
])
const closeSpecialtyModal = () => {
  showSpecialtyModal.value = false
}

const closeServiceModal = () => {
  showServiceModal.value = false
}








const fetchSpecialties = async () => {
  try {
    specialties.value = await $fetch('/api/MechanicSpecialty/GetSpecialties')
  } catch (error) {
    alert('Błąd podczas ładowania specjalności')
  }
}


const fetchServices = async () => {
  try {
    services.value = await $fetch('/api/Service')
  } catch (error) {
    alert('Błąd podczas ładowania serwisów')
  }
}


const addSpecialty = async () => {
  try {
    await $fetch('/api/MechanicSpecialty/AddSpecialty', {
      method: 'POST',
      body: JSON.stringify(newSpecialty.value),
      headers: { 'Content-Type': 'application/json' }
    })
    await fetchSpecialties()
    closeSpecialtyModal()
  } catch (error) {
    alert('Błąd podczas dodawania specjalności')
  }
}

const addService = async () => {
  try {
    const serviceData = {
      ...newService.value
    }
    await $fetch('/api/Service/AddService', {
      method: 'POST',
      body: JSON.stringify(serviceData),
      headers: { 'Content-Type': 'application/json' }
    })
    await fetchServices()
    closeServiceModal()
  } catch (error) {
    alert('Błąd podczas dodawania serwisu')
  }
}



onMounted(() => {
  fetchSpecialties()
  fetchServices()
})

</script>

<style scoped>
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
  color: #000000;
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
.v-tabs {
  align-items: center; /* Wyśrodkowanie w pionie */
  display: flex;
  justify-content: center;
}

/* Styl aktywnej zakładki */
.v-tab.v-tab--selected {
  background-color: #a0a0a0 !important; /* Ciemniejszy kolor aktywnej zakładki */
  color: #000 !important;
  font-weight: bold;
  border-radius: 5px; /* Zaokrąglenie dla lepszego efektu */
}

/* Styl nieaktywnej zakładki */
.v-tab {
  color: #000000;
  transition: background 0.3s ease;
}

/* Hover dla efektu */
.v-tab:hover {
  background-color: #c0c0c0;
}
</style>
