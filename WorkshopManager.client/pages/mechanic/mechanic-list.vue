<template>
    <v-card style="height: 100%; width: 100%;">
      <v-breadcrumbs :items="items">
      </v-breadcrumbs>
      <v-row>
        <v-col cols="12" class="text-left">
          <h1>Lista mechaników</h1>
        </v-col>
        <v-col cols="12" class="text-left">
          <v-btn style="background-color: #4caf50;" @click="openMechanicModal"><v-icon>mdi-plus</v-icon>Dodaj nowego mechanika</v-btn>
      </v-col>
      </v-row>
        <v-row>

        <v-col>

    <v-row>
      <v-col cols="12" md="6" v-for="mechanic in mechanics" :key="mechanic.id">
        <v-list-item>
            <v-list-item-title>
              <strong>{{ mechanic.firstName }} {{ mechanic.lastName }}</strong>
              <NuxtLink :to="`/mechanic/${mechanic.id}`" class="clickable-icon">
                <v-icon>mdi-eye</v-icon>
              </NuxtLink>
              <v-icon @click="editItem(mechanic)" class="clickable-icon">mdi-pencil</v-icon>
              <v-icon @click="deleteItem(mechanic.id)" class="clickable-icon">mdi-delete</v-icon>
            </v-list-item-title>
            <v-list-item-subtitle>
              Specjalność: {{ getSpecialtyName(mechanic.specialtyId) }}, Doświadczenie: {{ mechanic.experienceLevel }} lat
            </v-list-item-subtitle>
        </v-list-item>
      </v-col>
    </v-row>

        </v-col>
        
       
          
       
      </v-row>
  
      <!-- Modal -->
      <v-dialog v-model="showMechanicModal" persistent max-width="800">
        <v-card>
          <v-card-title class="headline">Dodaj Mechanika</v-card-title>
          <v-card-text>
            <v-form @submit.prevent="addMechanic">

              <v-row>
                <v-col>
                  <v-text-field variant="outlined"
                label="Imię"
                v-model="newMechanic.firstName"
                required
              ></v-text-field>
              <v-text-field variant="outlined"
                label="Numer telefonu"
                v-model="newMechanic.phoneNumber"
                required
              ></v-text-field>
              <v-text-field variant="outlined"
                label="Poziom doświadczenia"
                type="number"
                v-model="newMechanic.experienceLevel"
                required
              ></v-text-field>
                </v-col>
                <v-col>
                  <v-text-field variant="outlined"
                label="Nazwisko"
                v-model="newMechanic.lastName"
                required
              ></v-text-field>
              <v-select variant="outlined"
                :items="specialties"
                item-value="id"
                item-title="specialtyName"
                v-model="newMechanic.specialtyId"
                label="Specjalność"
                required
              ></v-select>
              <v-text-field variant="outlined"
                label="Data zatrudnienia"
                type="date"
                v-model="newMechanic.dateJoined"
                required
              ></v-text-field>
                </v-col>
              </v-row>



  

  

  

  

  

  
            </v-form>
          </v-card-text>

          <v-card-actions>
    
      <v-btn style="background-color: #f15c5c;" @click="closeMechanicModal">Anuluj</v-btn>
      <v-btn style="background-color: #4caf50;" type="submit">Dodaj Mechanika</v-btn>
    </v-card-actions>
        </v-card>
      </v-dialog>
    </v-card>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue'
  
  definePageMeta({
    middleware: 'auth',
    auth: {
      roles: ["Administrator", "Starszy Mechanik", "Młodszy Mechanik"]
    }
  })
  
  const mechanics = ref([])
  const specialties = ref([])
  const showMechanicModal = ref(false)
  const items = ref([
  {
    title: 'Dashboard',
    disabled: false,
    to: '/',
  },
  {
    title: 'Pracownicy',
    disabled: true,
    to: 'mechanic/mechanic-list',
  },
]);
  const newMechanic = ref({
    firstName: '',
    lastName: '',
    phoneNumber: '',
    specialtyId: '',
    experienceLevel: 0,
    dateJoined: ''
  })
  
  const openMechanicModal = () => {
    showMechanicModal.value = true
  }
  
  const closeMechanicModal = () => {
    showMechanicModal.value = false
    newMechanic.value = {
      firstName: '',
      lastName: '',
      phoneNumber: '',
      specialtyId: '',
      experienceLevel: 0,
      dateJoined: ''
    }
  }
  
  const getSpecialtyName = (specialtyId) => {
    const specialty = specialties.value.find(s => s.id === specialtyId)
    return specialty ? specialty.specialtyName : 'Nieznana specjalność'
  }
  
  const fetchMechanics = async () => {
    try {
      mechanics.value = await $fetch('/api/Mechanic/GetMechanicsList')
    } catch (error) {
      alert('Błąd podczas ładowania listy mechaników')
    }
  }
  
  const fetchSpecialties = async () => {
    try {
      specialties.value = await $fetch('/api/MechanicSpecialty/GetSpecialties')
    } catch (error) {
      alert('Błąd podczas ładowania listy specjalności')
    }
  }
  
  const addMechanic = async () => {
    newMechanic.value.dateJoined = new Date(newMechanic.value.dateJoined).toISOString()
    try {
      await $fetch('/api/Mechanic', {
        method: 'POST',
        body: JSON.stringify(newMechanic.value),
        headers: { 'Content-Type': 'application/json' }
      })
      fetchMechanics()
      closeMechanicModal()
      alert('Mechanik dodany pomyślnie!')
    } catch (error) {
      alert('Błąd podczas dodawania mechanika')
    }
  }
  
  onMounted(async () => {
    await fetchMechanics()
    await fetchSpecialties()
  })
  </script>
  
  <style scoped>
  .text-right {
    text-align: right;
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
  color: #000000 ;
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
.v-list-item {
  background: rgb(185, 185, 185);
  color: #000000;
}
.v-list {
  background: #a0a0a0;;
}



  </style>
  