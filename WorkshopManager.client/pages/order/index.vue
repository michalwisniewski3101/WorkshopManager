<template>
    <div>
        <h1>Lista zamówień</h1>
        <ul>
            <li v-for="order in orders" :key="order.id">
                <strong>Zamówienie ID: {{ order.id }}</strong>
                <p>Data zamówienia: {{ new Date(order.orderDate).toLocaleDateString() }}</p>
                <p>Klient: {{ order.clientName }} - Telefon: {{ order.clientPhoneNumber }}</p>
                <p>Status: {{ getOrderStatusName(order.orderStatus) }}</p>
                <p>Opis: {{ order.description }}</p>
                <p>Szacowana data ukończenia: 
                    {{ order.estimatedCompletionDate ? new Date(order.estimatedCompletionDate).toLocaleDateString() : "Brak" }}
                </p>
                <p>Całkowity koszt: {{ order.totalCost ? `${order.totalCost} PLN` : "Nie ustalono" }}</p>
            </li>
        </ul>

        <button @click="openOrderModal">Dodaj nowe zamówienie</button>

        <div v-if="showOrderModal" class="modal">
    <div class="modal-content">
        <h2>Dodaj Zamówienie</h2>
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
            <div>
                <label for="clientCode">Kod klienta:</label>
                <input type="text" v-model="newOrder.clientCode" id="clientCode" />
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

            <!-- Opcjonalne dane -->
            <div>
                <label for="serviceScheduleIds">Usługi (opcjonalne):</label>
                <select v-model="newOrder.serviceScheduleIds" id="serviceScheduleIds" multiple>
                    <option v-for="service in services" :key="service.id" :value="service.id">
                        {{ service.name }}
                    </option>
                </select>
            </div>

            <!-- Opis zamówienia -->
            <div>
                <label for="description">Opis zamówienia:</label>
                <textarea v-model="newOrder.description" id="description"></textarea>
            </div>

            <!-- Daty -->
            <div>
                <label for="orderDate">Data zamówienia:</label>
                <input type="date" v-model="newOrder.orderDate" id="orderDate" required />
            </div>
            <div>
                <label for="estimatedCompletionDate">Szacowana data ukończenia:</label>
                <input type="date" v-model="newOrder.estimatedCompletionDate" id="estimatedCompletionDate" />
            </div>

            <!-- Przyciski -->
            <button type="submit">Dodaj Zamówienie</button>
            <button @click="closeOrderModal" type="button">Anuluj</button>
        </form>
    </div>
</div>

    </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'

definePageMeta({
    middleware: 'auth',
    auth: {
        roles: ["Administrator", "Starszy Mechanik", "Młodszy Mechanik"]
    }
})

const orders = ref([])
const showOrderModal = ref(false)

const newOrder = ref({
    clientName: '',
    clientPhoneNumber: '',
    description: '',
    orderDate: '',
    estimatedCompletionDate: null
})

const openOrderModal = () => {
    showOrderModal.value = true
}

const closeOrderModal = () => {
    showOrderModal.value = false
    newOrder.value = {
        clientName: '',
        clientPhoneNumber: '',
        description: '',
        orderDate: '',
        estimatedCompletionDate: null
    }
}

const getOrderStatusName = (status) => {
    const statuses = {
        0: "Nowe",
        1: "W trakcie realizacji",
        2: "Zakończone",
        3: "Anulowane"
    }
    return statuses[status] || "Nieznany status"
}

const fetchOrders = async () => {
    try {
        orders.value = await $fetch('/api/Order')
    } catch (error) {
        alert('Błąd podczas ładowania listy zamówień')
    }
}

const addOrder = async () => {
    newOrder.value.orderDate = new Date(newOrder.value.orderDate).toISOString()
    if (newOrder.value.estimatedCompletionDate) {
        newOrder.value.estimatedCompletionDate = new Date(newOrder.value.estimatedCompletionDate).toISOString()
    }

    try {
        await $fetch('/api/Order', {
            method: 'POST',
            body: JSON.stringify(newOrder.value),
            headers: { 'Content-Type': 'application/json' }
        })
        fetchOrders()
        closeOrderModal()
        alert('Zamówienie dodane pomyślnie!')
    } catch (error) {
        alert('Błąd podczas dodawania zamówienia')
    }
}

onMounted(async () => {
    await fetchOrders()
})
</script>

<style>
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
