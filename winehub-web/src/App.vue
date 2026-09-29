<script setup>
import { onMounted, ref } from 'vue';

const API = 'http://localhost:5001/api';

const orders = ref([]);
const stock = ref([]);
const customers = ref([]);
const products = ref([]);
const customerId = ref('');
const orderLines = ref([{ productId: '', quantity: 1 }]);
const error = ref('');

function addOrderLine() {
  orderLines.value.push({ productId: '', quantity: 1 });
}

function removeOrderLine(index) {
  if (orderLines.value.length > 1) {
    orderLines.value.splice(index, 1);
  }
}

async function load() {
  orders.value = await fetch(`${API}/orders`).then((response) => response.json());
  stock.value = await fetch(`${API}/inventory`).then((response) => response.json());
  customers.value = await fetch(`${API}/customers`).then((response) => response.json());
  products.value = await fetch(`${API}/products`).then((response) => response.json());
}

async function create() {
  error.value = '';

  const validLines = orderLines.value
    .filter((line) => line.productId !== '')
    .map((line) => ({
      productId: Number(line.productId),
      quantity: Number(line.quantity),
    }));

  if (!customerId.value) {
    error.value = 'Vælg en kunde.';
    return;
  }

  if (!validLines.length) {
    error.value = 'Tilføj mindst én ordrelinje.';
    return;
  }

  const response = await fetch(`${API}/orders`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      customerId: Number(customerId.value),
      orderLines: validLines,
    }),
  });

  if (!response.ok) {
    error.value = (await response.json()).message;
    return;
  }

  customerId.value = '';
  orderLines.value = [{ productId: '', quantity: 1 }];
  await load();
}

onMounted(load);
</script>

<template>
  <main>
    <header class="topbar">
      <div>
        <p class="eyebrow">WineHub</p>
        <h1>Opgave 4</h1>
      </div>
      <span class="chip">EF Core · SQL Server · transaktioner</span>
    </header>

    <section class="overview-grid">
      <div class="overview-card">
        <span class="overview-label">Antal ordrer</span>
        <strong>{{ orders.length }}</strong>
      </div>
      <div class="overview-card">
        <span class="overview-label">Varer på lager</span>
        <strong>{{ stock.reduce((total, item) => total + Number(item.quantity || 0), 0) }}</strong>
      </div>
      <div class="overview-card">
        <span class="overview-label">Produkter</span>
        <strong>{{ products.length }}</strong>
      </div>
    </section>

    <section>
      <h2>Opret ordre</h2>

      <form @submit.prevent="create">
        <select v-model="customerId" required>
          <option disabled value="">Kunde</option>
          <option v-for="customer in customers" :key="customer.id" :value="customer.id">
            {{ customer.name }}
          </option>
        </select>

        <div class="order-lines">
          <div v-for="(line, index) in orderLines" :key="index" class="order-line-row">
            <select v-model="line.productId" required>
              <option disabled value="">Produkt</option>
              <option v-for="product in products" :key="product.id" :value="product.id">
                {{ product.name }} · {{ product.price }} kr.
              </option>
            </select>

            <input v-model.number="line.quantity" type="number" min="1" />

            <button
              v-if="orderLines.length > 1"
              type="button"
              class="small-button secondary-button"
              @click="removeOrderLine(index)"
            >
              Fjern
            </button>
          </div>
        </div>

        <div class="form-actions">
          <button type="button" class="small-button secondary-button" @click="addOrderLine">
            Tilføj linje
          </button>
          <button type="submit">Opret ordre</button>
        </div>
      </form>

      <p>{{ error }}</p>
    </section>

    <section>
      <h2>Lager</h2>

      <table>
        <thead>
          <tr>
            <th>Produkt</th>
            <th>Antal</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(item, index) in stock" :key="item.productName || index">
            <td>{{ item.productName }}</td>
            <td>{{ item.quantity }}</td>
          </tr>
        </tbody>
      </table>
    </section>

    <section>
      <h2>Ordrer</h2>

      <table v-if="orders.length">
        <thead>
          <tr>
            <th>Ordre</th>
            <th>Kunde</th>
            <th>Status</th>
            <th>Dato</th>
            <th>Beløb</th>
          </tr>
        </thead>
        <tbody>
          <template v-for="order in orders" :key="order.id">
            <tr>
              <td>#{{ order.id }}</td>
              <td>{{ order.customerNameSnapshot }}</td>
              <td>
                <span
                  class="status-badge"
                  :class="{
                    success: order.status === 'Paid' || order.status === 'Completed',
                    warning: order.status === 'Created' || order.status === 'Processing'
                  }"
                >
                  {{ order.status }}
                </span>
              </td>
              <td>{{ new Date(order.createdAt).toLocaleDateString('da-DK') }}</td>
              <td>{{ order.total }} kr.</td>
            </tr>

            <tr v-if="order.orderLines && order.orderLines.length">
              <td colspan="5">
                <table class="order-lines-table">
                  <thead>
                    <tr>
                      <th>Produkt</th>
                      <th>Antal</th>
                      <th>Pris</th>
                      <th>Linje total</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr v-for="line in order.orderLines" :key="`${order.id}-${line.productId}`">
                      <td>{{ line.productNameSnapshot }}</td>
                      <td>{{ line.quantity }}</td>
                      <td>{{ line.unitPrice }} kr.</td>
                      <td>{{ line.lineTotal }} kr.</td>
                    </tr>
                  </tbody>
                </table>
              </td>
            </tr>
          </template>
        </tbody>
      </table>

      <p v-else>Ingen ordrer endnu.</p>
    </section>
  </main>
</template>

