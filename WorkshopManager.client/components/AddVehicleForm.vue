<template>
    <v-container>
      <v-row>
        <v-col cols="12" class="text-left">
          <v-btn color="#4caf50" @click="openVehicleModal"><v-icon>mdi-plus</v-icon>Dodaj nowy pojazd</v-btn>
        </v-col>
      </v-row>
  
      <!-- Modal -->
      <v-dialog v-model="showVehicleModal" persistent max-width="800">
        <v-card>
          <v-card-title >Dodaj Pojazd</v-card-title>
          <v-card-text>
            <v-form @submit.prevent="addVehicle">
              <v-row>
                <v-col cols="6">
                  <v-text-field variant="outlined"
                label="Marka"
                v-model="newVehicle.make"
                required
              ></v-text-field>
              <v-text-field variant="outlined"
                label="Rok produkcji"
                type="number"
                v-model="newVehicle.year"
                required
              ></v-text-field>
              <v-text-field variant="outlined"
                label="VIN"
                v-model="newVehicle.vin"
                required
              ></v-text-field>
                </v-col >







                <v-col cols="6">
                  <v-text-field variant="outlined"
                label="Model"
                v-model="newVehicle.model"
                required
              ></v-text-field>
              <v-text-field variant="outlined"
                label="Numer rejestracyjny"
                v-model="newVehicle.licensePlate"
                required
              ></v-text-field>
                </v-col>

              </v-row>

              <v-row>
                <v-col cols="6">
                  <v-text-field variant="outlined"
                label="Imię i nazwisko właściciela"
                v-model="newVehicle.ownerName"
                required
              ></v-text-field>
  
              <v-text-field variant="outlined"
                label="Numer telefonu właściciela"
                type="tel"
                v-model="newVehicle.ownerPhoneNumber"
                required
              ></v-text-field>
                </v-col >







                <v-col cols="6">
                  <v-text-field variant="outlined"
                label="Email właściciela"
                type="email"
                v-model="newVehicle.ownerEmail"
                required
              ></v-text-field>
  
              <v-text-field variant="outlined"
                label="Adres właściciela"
                v-model="newVehicle.ownerAddress"
                required
              ></v-text-field>
                </v-col>

              </v-row>
            </v-form>
          </v-card-text>
          <v-card-actions>
      
            <v-btn @click="closeVehicleModal" style="background-color: #f15c5c;">Anuluj</v-btn>
            <v-btn style="background-color: #4caf50;" type="submit">Dodaj Pojazd</v-btn>

    </v-card-actions>
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
  