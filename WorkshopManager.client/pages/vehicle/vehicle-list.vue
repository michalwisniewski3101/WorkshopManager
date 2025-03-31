<template>
  <v-card style="height: 100%; width: 100%;">
    <v-breadcrumbs :items="items">
    </v-breadcrumbs>
    <v-row>
      <v-col cols="12" class="text-left">
        <h1>Lista pojazdów</h1>
      </v-col>
      <v-col cols="6" class="text-left">
        <AddVehicleForm @refresh="fetchVehicles" />
      </v-col>
      <v-col cols="6" class="text-right">
        <v-text-field variant="outlined"
          v-model="search"
          label="Szukaj"
          prepend-inner-icon="mdi-magnify"
          hide-details
          single-line
        ></v-text-field>
      </v-col>
    </v-row>

    <v-data-table
      :headers="tableHeaders"
      :items="vehicles"
      item-key="id"
      class="elevation-1"
      :search="search"
    >
      <template v-slot:item.make="{ item }">
        {{ item.make }}
      </template>
      <template v-slot:item.model="{ item }">
        {{ item.model }}
      </template>
      <template v-slot:item.year="{ item }">
        {{ item.year }}
      </template>
      <template v-slot:item.licensePlate="{ item }">
        {{ item.licensePlate }}
      </template>
      <template v-slot:item.actions="{ item }">
        <NuxtLink :to="`/vehicle/${item.id}`" class="clickable-icon">
          <v-icon>mdi-eye</v-icon>
        </NuxtLink>
        <v-icon @click="editItem(item)" class="clickable-icon">mdi-pencil</v-icon>
        <v-icon @click="deleteItem(item.id)" class="clickable-icon">mdi-delete</v-icon>
      </template>
    </v-data-table>

    <v-dialog v-model="showEditModal" persistent max-width="800">
      <v-card>
        <v-card-title>Edytuj Pojazd</v-card-title>
        <v-card-text>
          <v-form v-if="editedVehicle" @submit.prevent="updateVehicle">
            <v-row>
              <v-col cols="6">
                <v-text-field variant="outlined"
                  label="Marka"
                  v-model="editedVehicle.make"
                  required
                ></v-text-field>
                <v-text-field variant="outlined"
                  label="Rok produkcji"
                  type="number"
                  v-model="editedVehicle.year"
                  required
                ></v-text-field>
                <v-text-field variant="outlined"
                  label="VIN"
                  v-model="editedVehicle.vin"
                  required
                ></v-text-field>
              </v-col>
              <v-col cols="6">
                <v-text-field variant="outlined"
                  label="Model"
                  v-model="editedVehicle.model"
                  required
                ></v-text-field>
                <v-text-field variant="outlined"
                  label="Numer rejestracyjny"
                  v-model="editedVehicle.licensePlate"
                  required
                ></v-text-field>
              </v-col>
            </v-row>
             <v-row>
               <v-col cols="6">
                 <v-text-field variant="outlined"
                  label="Imię i nazwisko właściciela"
                   v-model="editedVehicle.ownerName"
                  required
                 ></v-text-field>
                 <v-text-field variant="outlined"
                   label="Numer telefonu właściciela"
                  type="tel"
                  v-model="editedVehicle.ownerPhoneNumber"
                   required
                 ></v-text-field>
               </v-col>
               <v-col cols="6">
                 <v-text-field variant="outlined"
                  label="Email właściciela"
                   type="email"
                  v-model="editedVehicle.ownerEmail"
                  required
                 ></v-text-field>
                 <v-text-field variant="outlined"
                   label="Adres właściciela"
                  v-model="editedVehicle.ownerAddress"
                  required
                 ></v-text-field>
               </v-col>
             </v-row>
             <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn @click="closeEditModal" style="background-color: #f15c5c;">Anuluj</v-btn>
                <v-btn style="background-color: #4caf50;" type="submit">Zapisz Zmiany</v-btn>
              </v-card-actions>
          </v-form>
        </v-card-text>
        </v-card>
    </v-dialog>

  </v-card>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import AddVehicleForm from '@/components/AddVehicleForm.vue' 
import { useToast } from 'vue-toastification' 

definePageMeta({
  layout: 'default',
  middleware: 'auth',
  auth: {
    roles: ["Administrator", "Starszy Mechanik", "Młodszy Mechanik"]
  },
})

const vehicles = ref([])
const search = ref("")
const toast = useToast() 

const showEditModal = ref(false)
const editedVehicle = ref(null) 

const tableHeaders = [
  { title: 'Marka', key: 'make' },
  { title: 'Model', key: 'model' },
  { title: 'Rok', key: 'year' },
  { title: 'Rejestracja', key: 'licensePlate' },
  { title: 'Akcje', key: 'actions', sortable: false }
]

const items = ref([
{
  title: 'Dashboard',
  disabled: false,
  to: '/',
},
{
  title: 'Pojazdy',
  disabled: true,
  to: '/vehicle/vehicle-list',
},
]);

const fetchVehicles = async () => {
  try {
    vehicles.value = await $fetch('/api/Vehicle')
  } catch (error) {
    toast.error('Błąd podczas ładowania listy pojazdów') 

  }
}

const editItem = (item) => {
  editedVehicle.value = { ...item }
  showEditModal.value = true
}


const closeEditModal = () => {
  showEditModal.value = false
  editedVehicle.value = null
}


const updateVehicle = async () => {
  if (!editedVehicle.value) return;

  try {
    await $fetch(`/api/Vehicle/${editedVehicle.value.id}`, { 
      method: 'PUT', 
      body: JSON.stringify(editedVehicle.value),
      headers: { 'Content-Type': 'application/json' }
    })
    toast.success('Dane pojazdu zaktualizowane pomyślnie!')
    closeEditModal()
    await fetchVehicles() 
  } catch (error) {
    toast.error('Błąd podczas aktualizacji danych pojazdu')
    console.error('Błąd aktualizacji:', error); 
  }
}


const deleteItem = async (id) => {
 console.log('Usuń pojazd ID:', id)
 if (confirm(`Czy na pewno chcesz usunąć pojazd o ID: ${id}?`)) {
    try {
      await $fetch(`/api/Vehicle/${id}`, { method: 'DELETE' });
      toast.success('Pojazd usunięty pomyślnie!');
      await fetchVehicles(); 
    } catch (error) {
      toast.error('Błąd podczas usuwania pojazdu.');
      console.error('Błąd usuwania:', error);
    }
  }
}

onMounted(fetchVehicles)
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
:deep(.v-pagination .v-btn:not(.v-btn--active)) { 
  background-color: #c0c0c0 !important; 
  color: #000000 !important; 
  margin: 0 2px; 
}


:deep(.v-pagination .v-pagination__item--active .v-btn) { 
  background-color: #4caf50 !important; 
  color: white !important; 
}


:deep(.v-pagination .v-pagination__navigation .v-btn[disabled]) {
  background-color: #e0e0e0 !important; 
  color: #a0a0a0 !important; 
  opacity: 0.6; 
}

:deep(.v-pagination .v-btn:not(.v-btn--active):not([disabled]):hover) {
  background-color: #a8a8a8 !important; 
}
  </style>
  