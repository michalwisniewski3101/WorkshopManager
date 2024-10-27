<template>
    <div>
        <h1>Lista mechaników</h1>
        <ul>
            <li v-for="mechanic in mechanics" :key="mechanic.id">
                <strong>{{ mechanic.firstName }} {{ mechanic.lastName }}</strong> - Specjalność: {{
                    mechanic.specialtyId }}, Doświadczenie: {{ mechanic.experienceLevel }} lat
            </li>
        </ul>

        <button @click="openMechanicModal">Dodaj nowego mechanika</button>

        
        <div v-if="showMechanicModal" class="modal">
            <div class="modal-content">
                <h2>Dodaj Mechanika</h2>
                <form @submit.prevent="addMechanic">
                    <div>
                        <label for="firstName">Imię:</label>
                        <input type="text" v-model="newMechanic.firstName" id="firstName" required />
                    </div>
                    <div>
                        <label for="lastName">Nazwisko:</label>
                        <input type="text" v-model="newMechanic.lastName" id="lastName" required />
                    </div>
                    <div>
                        <label for="phoneNumber">Numer telefonu:</label>
                        <input type="tel" v-model="newMechanic.phoneNumber" id="phoneNumber" required />
                    </div>
                    <div>
                        <label for="specialty">Specjalność:</label>
                        <select v-model="newMechanic.specialtyId" id="specialty" required>
                            <option disabled value="">Wybierz specjalność</option>
                            <option v-for="specialty in specialties" :key="specialty.id" :value="specialty.id">
                                {{ specialty.specialtyName }}
                            </option>
                        </select>
                    </div>
                    <div>
                        <label for="experienceLevel">Poziom doświadczenia:</label>
                        <input type="number" v-model="newMechanic.experienceLevel" id="experienceLevel" required />
                    </div>
                    <div>
                        <label for="dateJoined">Data zatrudnienia:</label>
                        <input type="date" v-model="newMechanic.dateJoined" id="dateJoined" required />
                    </div>
                    <button type="submit">Dodaj Mechanika</button>
                    <button @click="closeMechanicModal" type="button">Anuluj</button>
                </form>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'

const mechanics = ref([])
const specialties = ref([])
const showMechanicModal = ref(false)

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
    // Dodanie daty w odpowiednim formacie
    newMechanic.value.dateJoined = new Date(newMechanic.value.dateJoined).toISOString();

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

<style>
/* Styl dla modali */
.modal {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.5);
    display: flex;
    align-items: center;
    justify-content: center;
}

.modal-content {
    background: white;
    padding: 20px;
    border-radius: 8px;
    width: 300px;
}
</style>
