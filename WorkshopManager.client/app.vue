<template>
  <div>
    <nav>
      <ul>
        <li>
          <NuxtLink to="/">Strona główna</NuxtLink>
        </li>
        <li>
          <NuxtLink to="/vehicle/add-vehicle">Dodaj pojazd</NuxtLink>
        </li>
        <li>
          <NuxtLink to="/vehicle/vehicle-list">Lista pojazdów</NuxtLink>
        </li>
      </ul>
    </nav>
    <NuxtPage />
  </div>
</template>

<script setup>
</script>

<style>
nav ul {
  display: flex;
  gap: 1rem;
  list-style-type: none;
}

nav ul li {
  display: inline;
}
</style>
