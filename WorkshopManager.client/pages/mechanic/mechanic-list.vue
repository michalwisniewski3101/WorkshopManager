<template>
    <v-card style="height: 100%; width: 100%;">
      <v-breadcrumbs :items="items">
      </v-breadcrumbs>
      <v-row>
        <v-col cols="12" class="text-left">
          <h1>Lista mechaników</h1>
        </v-col>
        <v-col cols="12" class="text-left">
          <v-btn color="primary" @click="openMechanicModal">Dodaj nowego mechanika</v-btn>
      </v-col>
      </v-row>
        <v-row>

        <v-col>
          <v-list>
            <v-list-item
              v-for="mechanic in mechanics"
              :key="mechanic.id"
            >
              <v-list-item-content>
                <v-list-item-title>
                  <strong>{{ mechanic.firstName }} {{ mechanic.lastName }}</strong>                <NuxtLink :to="`/mechanic/${mechanic.id}`" class="clickable-icon">
          
          <v-icon>mdi-eye</v-icon>
           
      </NuxtLink>
                </v-list-item-title>
                <v-list-item-subtitle>
                  Specjalność: {{ getSpecialtyName(mechanic.specialtyId) }}, Doświadczenie: {{ mechanic.experienceLevel }} lat
               

                </v-list-item-subtitle>

              </v-list-item-content>
            </v-list-item>
            
          </v-list>
        </v-col>
        
       
          
       
      </v-row>
  
      <!-- Modal -->
      <v-dialog v-model="showMechanicModal" persistent max-width="500">
        <v-card>
          <v-card-title class="headline">Dodaj Mechanika</v-card-title>
          <v-card-text>
            <v-form @submit.prevent="addMechanic">
              <v-text-field
                label="Imię"
                v-model="newMechanic.firstName"
                required
              ></v-text-field>
  
              <v-text-field
                label="Nazwisko"
                v-model="newMechanic.lastName"
                required
              ></v-text-field>
  
              <v-text-field
                label="Numer telefonu"
                v-model="newMechanic.phoneNumber"
                required
              ></v-text-field>
  
              <v-select
                :items="specialties"
                item-value="id"
                item-title="specialtyName"
                v-model="newMechanic.specialtyId"
                label="Specjalność"
                required
              ></v-select>
  
              <v-text-field
                label="Poziom doświadczenia"
                type="number"
                v-model="newMechanic.experienceLevel"
                required
              ></v-text-field>
  
              <v-text-field
                label="Data zatrudnienia"
                type="date"
                v-model="newMechanic.dateJoined"
                required
              ></v-text-field>
  
              <v-row class="mt-4">
                <v-col cols="6">
                  
                </v-col>
                <v-col cols="6">
                  
                </v-col>
              </v-row>
            </v-form>
          </v-card-text>

          <v-card-actions>
    
      <v-btn color="grey" @click="closeMechanicModal">Anuluj</v-btn>
      <v-btn color="white" type="submit">Dodaj Mechanika</v-btn>
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
  </style>
  