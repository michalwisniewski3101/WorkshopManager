<template>
    <div>
        <h1>Witamy w aplikacji WorkshopManager</h1>
        <p>Ta aplikacja pozwala na zarządzanie warsztatem samochodowym.</p>
    </div>
</template>

<script setup>
definePageMeta({
  middleware: 'auth',
  auth: {
    roles: ["Administrator", "Starszy Mechanik", "Młodszy Mechanik", "Klient"]
  }
})
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