<template>
    <div>
        <h1>Witamy w aplikacji WorkshopManager</h1>
        <p>Ta aplikacja pozwala na zarządzanie warsztatem samochodowym.</p>
        <div>
            <NuxtLink to="/vehicle/add-vehicle">Dodaj nowy pojazd</NuxtLink>
        </div>
        <div>
            <NuxtLink to="/vehicle/vehicle-list">Zobacz listę pojazdów</NuxtLink>
        </div>
    </div>
</template>

<script setup>
</script>

<style scoped>
h1 {
    color: #333;
}

div {
    margin-bottom: 1rem;
}

a {
    color: #007bff;
    text-decoration: none;
}

a:hover {
    text-decoration: underline;
}
</style>