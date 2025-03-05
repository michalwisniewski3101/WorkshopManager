<script setup>
import { loadStripe } from '@stripe/stripe-js';
import { ref } from 'vue';

const props = defineProps({
  orderId: {
    type: String,
    required: true
  }
});

const stripe = await loadStripe('pk_test_51Qz4ISGgTr0OYHfgpRc8Vowoi0ldH33JzE1u3qI3GkQhSowWrYmD1kUN7ZJkoG95zMybxRcBm11BduJm3yAzxDhp00bpAPWuF8');
const needsInvoice = ref(false);

async function pay() {
  const res = await fetch('https://localhost:44347/api/Payment/create-checkout-session', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      NeedsInvoice: needsInvoice.value,
      OrderId: props.orderId
    })
  });

  const { url } = await res.json();
  window.location.href = url;
}
</script>

<template>
  <div>
    <label>
      <input type="checkbox" v-model="needsInvoice" />
      Chcę otrzymać fakturę
    </label>
    <button @click="pay">Zapłać</button>
  </div>
</template>
