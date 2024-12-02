<template>
    <div v-if="isOpen" class="modal-overlay">
        <div class="modal-content">
            <h2>Dodaj pojazd</h2>
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
                <div>
                    <label for="VIN">VIN:</label>
                    <input type="text" v-model="vehicle.vin" id="vin" required />
                </div>

                <!-- Dane właściciela -->
                <div>
                    <label for="ownerName">Imię i nazwisko właściciela:</label>
                    <input type="text" v-model="vehicle.ownerName" id="ownerName" required />
                </div>
                <div>
                    <label for="ownerPhoneNumber">Numer telefonu właściciela:</label>
                    <input type="tel" v-model="vehicle.ownerPhoneNumber" id="ownerPhoneNumber" required />
                </div>
                <div>
                    <label for="ownerEmail">Email właściciela:</label>
                    <input type="email" v-model="vehicle.ownerEmail" id="ownerEmail" required />
                </div>
                <div>
                    <label for="ownerAddress">Adres właściciela:</label>
                    <input type="text" v-model="vehicle.ownerAddress" id="ownerAddress" required />
                </div>

                <button type="submit">Dodaj pojazd</button>
                <button type="button" @click="closeModal">Anuluj</button>
            </form>
        </div>
    </div>
</template>

<script setup>
import { ref, defineEmits } from 'vue'
import { useToast } from 'vue-toastification'
const emits = defineEmits(['close', 'refresh'])
const toast = useToast()

const isOpen = ref(true)
const vehicle = ref({
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

const closeModal = () => {
    isOpen.value = false
    emits('close')
}

const submitForm = async () => {
    try {
        await $fetch('/api/Vehicle', {
            method: 'POST',
            body: vehicle.value
        })
        toast.success('Pojazd dodany pomyślnie!')
        emits('refresh') // Odświeżenie listy pojazdów
        closeModal()
    } catch (error) {
        toast.error('Błąd podczas dodawania pojazdu')
    }
}
</script>

<style scoped>
.modal-overlay {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
}

.modal-content {
    background: white;
    padding: 20px;
    border-radius: 8px;
    width: 400px;
    max-width: 90%;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}
</style>
