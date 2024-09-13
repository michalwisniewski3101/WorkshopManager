<template>
    <div>
        <h1>Dodaj pojazd</h1>
        <form @submit.prevent="submitForm">
            <div>
                <label for="make">Marka:</label>
                <input type="text" v-model="vehicle.make" id="make" required />
            </div>
            <div>
                <label for="model">Model:</label>
                <input type="text" v-model="vehicle.model" id="model" required />
            </div>
            <div>
                <label for="year">Rok produkcji:</label>
                <input type="number" v-model="vehicle.year" id="year" required />
            </div>
            <div>
                <label for="licensePlate">Numer rejestracyjny:</label>
                <input type="text" v-model="vehicle.licensePlate" id="licensePlate" required />
            </div>
            <button type="submit">Dodaj pojazd</button>
        </form>
    </div>
</template>

<script setup>
import { ref } from 'vue'

const vehicle = ref({
    make: '',
    model: '',
    year: '',
    licensePlate: ''
})

const submitForm = async () => {
    try {
        await $fetch('/api/vehicles', {
            method: 'POST',
            body: vehicle.value
        })
        alert('Pojazd dodany pomyślnie!')
    } catch (error) {
        alert('Błąd podczas dodawania pojazdu')
    }
}
</script>