<template>
    <v-container>
      <v-row>
        <v-col cols="12" class="text-right">
          <v-btn color="primary" @click="openVehicleModal">Dodaj nowy pojazd</v-btn>
        </v-col>
      </v-row>
  
      <!-- Modal -->
      <v-dialog v-model="showVehicleModal" persistent max-width="500">
        <v-card>
          <v-card-title class="text-h6">Dodaj Pojazd</v-card-title>
          <v-card-text>
            <v-form @submit.prevent="addVehicle">
              <v-text-field
                label="Marka"
                v-model="newVehicle.make"
                required
              ></v-text-field>
  
              <v-text-field
                label="Model"
                v-model="newVehicle.model"
                required
              ></v-text-field>
  
              <v-text-field
                label="Rok produkcji"
                type="number"
                v-model="newVehicle.year"
                required
              ></v-text-field>
  
              <v-text-field
                label="Numer rejestracyjny"
                v-model="newVehicle.licensePlate"
                required
              ></v-text-field>
  
              <v-text-field
                label="VIN"
                v-model="newVehicle.vin"
                required
              ></v-text-field>
  
              <v-text-field
                label="Imię i nazwisko właściciela"
                v-model="newVehicle.ownerName"
                required
              ></v-text-field>
  
              <v-text-field
                label="Numer telefonu właściciela"
                type="tel"
                v-model="newVehicle.ownerPhoneNumber"
                required
              ></v-text-field>
  
              <v-text-field
                label="Email właściciela"
                type="email"
                v-model="newVehicle.ownerEmail"
                required
              ></v-text-field>
  
              <v-text-field
                label="Adres właściciela"
                v-model="newVehicle.ownerAddress"
                required
              ></v-text-field>
  
              <v-row class="mt-4">
                <v-col cols="6">
                  <v-btn color="primary" block type="submit">Dodaj Pojazd</v-btn>
                </v-col>
                <v-col cols="6">
                  <v-btn block @click="closeVehicleModal">Anuluj</v-btn>
                </v-col>
              </v-row>
            </v-form>
          </v-card-text>
        </v-card>
      </v-dialog>
    </v-container>
  </template>
  
  <script setup>
import { ref, defineEmits } from 'vue'
  import { useToast } from 'vue-toastification'
  

    const emit = defineEmits(['refresh'])
  const showVehicleModal = ref(false)
  const toast = useToast()
  
  const newVehicle = ref({
    make: '',
    model: '',
    year: '',
    licensePlate: '',
    vin: '',
    ownerName: '',
    ownerPhoneNumber: '',
    ownerEmail: '',
    ownerAddress: ''
  })
  
  const openVehicleModal = () => {
    showVehicleModal.value = true
  }
  
  const closeVehicleModal = () => {
    showVehicleModal.value = false
    newVehicle.value = {
      make: '',
      model: '',
      year: '',
      licensePlate: '',
      vin: '',
      ownerName: '',
      ownerPhoneNumber: '',
      ownerEmail: '',
      ownerAddress: ''
    }
  }
  
  const addVehicle = async () => {
    try {
      await $fetch('/api/Vehicle', {
        method: 'POST',
        body: JSON.stringify(newVehicle.value),
        headers: { 'Content-Type': 'application/json' }
      })
      toast.success('Pojazd dodany pomyślnie!')
      closeVehicleModal()
      emit('refresh')
    } catch (error) {
      toast.error('Błąd podczas dodawania pojazdu')
    }
  }
  </script>
  
  <style scoped>
  .text-right {
    text-align: right;
  }
  </style>
  