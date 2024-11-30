<template>
    <div>
      <h1>Dodaj Zamówienie</h1>
      <form @submit.prevent="addOrder">
            <!-- Dane klienta -->
            <div>
                <label for="clientName">Imię i nazwisko klienta:</label>
                <input type="text" v-model="newOrder.clientName" id="clientName" required />
            </div>
            <div>
                <label for="clientPhoneNumber">Numer telefonu klienta:</label>
                <input type="tel" v-model="newOrder.clientPhoneNumber" id="clientPhoneNumber" required />
            </div>
            <div>
                <label for="clientEmail">E-mail klienta:</label>
                <input type="email" v-model="newOrder.clientEmail" id="clientEmail" />
            </div>
            <div>
                <label for="clientAddress">Adres klienta:</label>
                <input type="text" v-model="newOrder.clientAddress" id="clientAddress" />
            </div>


            <!-- Dane pojazdu -->
            <div>
                <label for="vin">Numer VIN:</label>
                <input type="text" v-model="newOrder.vin" id="vin" required />
            </div>
            <div>
                <label for="make">Marka pojazdu:</label>
                <input type="text" v-model="newOrder.make" id="make" required />
            </div>
            <div>
                <label for="model">Model pojazdu:</label>
                <input type="text" v-model="newOrder.model" id="model" required />
            </div>
            <div>
                <label for="year">Rok produkcji:</label>
                <input type="number" v-model="newOrder.year" id="year" required />
            </div>
            <div>
                <label for="licensePlate">Tablica rejestracyjna:</label>
                <input type="text" v-model="newOrder.licensePlate" id="licensePlate" required />
            </div>

            
                    

            <!-- Opis zamówienia -->
            <div>
                <label for="description">Opis zamówienia:</label>
                <textarea v-model="newOrder.description" id="description"></textarea>
            </div>



            <!-- Przyciski -->
            <button type="submit">Dodaj Zamówienie</button>
            <button @click="closeOrderModal" type="button">Anuluj</button>
        <NuxtLink to="/order"><button type="button">Anuluj</button></NuxtLink>
      </form>
      <service-schedule-form  />
    </div>
</template>
  
  <script setup>
  import { ref } from 'vue'
  import { useRouter } from 'vue-router'
  
  const router = useRouter()
  const newOrder = ref({
    services: [],
    description: '',
    clientName: '',
    clientPhoneNumber: '',
    ClientEmail: '',
    ClientAddress: '',
    VIN: '',
    Make: '',
    Model: '',
    Year: '',
    LicensePlate: '',
  })
  
  const addOrder = async () => {
    try {
      newOrder.value.orderDate = new Date(newOrder.value.orderDate).toISOString()
      if (newOrder.value.estimatedCompletionDate) {
        newOrder.value.estimatedCompletionDate = new Date(newOrder.value.estimatedCompletionDate).toISOString()
      }
  
      await $fetch('/api/Order', {
        method: 'POST',
        body: JSON.stringify(newOrder.value),
        headers: { 'Content-Type': 'application/json' }
      })
      alert('Zamówienie dodane pomyślnie!')
      router.push('/order')
    } catch (error) {
      alert('Błąd podczas dodawania zamówienia')
    }
  }

  </script>







  