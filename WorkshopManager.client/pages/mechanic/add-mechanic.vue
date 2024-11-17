<template>
    <div>
        <h1>Dodaj mechanika</h1>
        <form @submit.prevent="submitForm">
            <div>
                <label for="firstName">Imię:</label>
                <input type="text" v-model="mechanic.firstName" id="firstName" required />
            </div>
            <div>
                <label for="lastName">Nazwisko:</label>
                <input type="text" v-model="mechanic.lastName" id="lastName" required />
            </div>
            <div>
                <label for="phoneNumber">Numer telefonu:</label>
                <input type="tel" v-model="mechanic.phoneNumber" id="phoneNumber" required />
            </div>
            <div>
                <label for="specialty">Specjalność:</label>
                <input type="text" v-model="mechanic.specialty" id="specialty" required />
            </div>
            <div>
                <label for="experienceLevel">Poziom doświadczenia:</label>
                <input type="number" v-model="mechanic.experienceLevel" id="experienceLevel" required />
            </div>
            <div>
                <label for="dateJoined">Data zatrudnienia:</label>
                <input type="date" v-model="mechanic.dateJoined" id="dateJoined" required />
            </div>

            <button type="submit">Dodaj mechanika</button>
        </form>
    </div>
</template>

<script setup>
import { ref } from 'vue'
definePageMeta({
  middleware: 'auth',
  auth: {
    roles: ["Administrator", "Starszy Mechanik"]
  }
})
const mechanic = ref({
    firstName: '',
    lastName: '',
    phoneNumber: '',
    specialty: '',
    experienceLevel: 0,
    dateJoined: ''
})

const submitForm = async () => {
    try {
        await $fetch('/api/Mechanic', {
            method: 'POST',
            body: mechanic.value
        })
        alert('Mechanik dodany pomyślnie!')
    } catch (error) {
        alert('Błąd podczas dodawania mechanika')
    }
}
</script>
