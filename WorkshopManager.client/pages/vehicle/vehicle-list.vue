<template>
    <div>
        <h1>Lista pojazdów</h1>
        <ul>
            <li v-for="vehicle in vehicles" :key="vehicle.id">
                <strong>{{ vehicle.make }} {{ vehicle.model }}</strong> - Rok: {{ vehicle.year }}, Rejestracja: {{
                    vehicle.licensePlate }}
            </li>
        </ul>
    </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
definePageMeta({
  middleware: 'auth'
})
const vehicles = ref([])

onMounted(async () => {
    try {
        vehicles.value = await $fetch('/api/Vehicle')
    } catch (error) {
        alert('Błąd podczas ładowania listy pojazdów')
    }
})
</script>