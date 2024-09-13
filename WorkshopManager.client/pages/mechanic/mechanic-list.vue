<template>
    <div>
        <h1>Lista mechaników</h1>
        <ul>
            <li v-for="mechanic in mechanics" :key="mechanic.id">
                <strong>{{ mechanic.firstName }} {{ mechanic.lastName }}</strong> - Specjalność: {{
                    mechanic.specialty }}, Doświadczenie: {{ mechanic.experienceLevel }} lat
            </li>
        </ul>
    </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'

const mechanics = ref([])

onMounted(async () => {
    try {
        mechanics.value = await $fetch('/api/Mechanic')
    } catch (error) {
        alert('Błąd podczas ładowania listy mechaników')
    }
})
</script>
