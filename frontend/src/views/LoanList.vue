<template>
  <!-- Фильтры -->
  <div class="filters">
    <h2>Фильтры</h2>
    <div class="filters-item">
      <label>
        Статус:
        <select v-model="filterStatus" @change="fetchLoans">
          <option value="0">Все</option>
          <option value="1">Опубликованные</option>
          <option value="2">Снятые с публикации</option>
        </select>
      </label>
    </div>
    <div class="filters-item">
      <label>
        Сумма от:
        <input type="number" v-model.number="filterMinAmount" @input="fetchLoans" />
      </label>

      <label>
        до:
        <input type="number" v-model.number="filterMaxAmount" @input="fetchLoans" />
      </label>
    </div>
    <div class="filters-item">
      <label>
        Срок займа от:
        <input type="number" v-model.number="filterMinTerm" @input="fetchLoans" />
      </label>
      <label>
        до:
        <input type="number" v-model.number="filterMaxTerm" @input="fetchLoans" />
      </label>

    </div>
  </div>

  <!-- Таблица с заявками -->
  <div class="loan-list">
    <h2>Список заявок</h2>
    <table>
      <thead>
        <tr>
          <th>Номер заявки</th>
          <th>Сумма</th>
          <th>Срок займа</th>
          <th>Процентная ставка</th>
          <th>Статус</th>
          <th>Дата создания</th>
          <th>Изменение статуса</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="loan in loans" :key="loan.id">
          <td>{{ loan.number }}</td>
          <td>{{ loan.amount }}</td>
          <td>{{ loan.termValue }}</td>
          <td>{{ loan.interestValue }}</td>
          <td>{{ loan.status === 1 ? 'опубликована' : 'снята с публикации' }}</td>
          <td>{{ formatDate(loan.createdAt) }}</td>
          <td>
            <button @click="toggleStatus(loan)">
              {{ loan.status ? 'Снять с публикации' : 'Опубликовать' }}
            </button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';

interface Loan {
  id: number;
  number: string;
  amount: number;
  termValue: number;
  interestValue: number;
  status: number;
  createdAt: string;
}

const loans = ref<Loan[]>([]);

function formatDate(dateStr: string): string {
  const date = new Date(dateStr);
  return date.toLocaleDateString('ru-RU', {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit'
  });
}

async function toggleStatus(loan: Loan) {
  const response = await fetch(`http://localhost:5007/api/loans/${loan.id}/change-status`, {
    method: 'PATCH'
  });

  if (!response.ok) {
    console.error('Ошибка при смене статуса:', response.status);
    return;
  }

  const updatedLoan = await response.json();
  loan.status = updatedLoan.status;
}

// Реактивные переменные для фильтрации

const filterStatus = ref<string>('0');
const filterMinAmount = ref<number | null>(null);
const filterMaxAmount = ref<number | null>(null);
const filterMinTerm = ref<number | null>(null);
const filterMaxTerm = ref<number | null>(null);

async function fetchLoans() {

  const params = new URLSearchParams();
  // статус
  if (filterStatus.value === '1') params.append('status', '1'); // опубликованные
  else if (filterStatus.value === '2') params.append('status', '0'); // снятые

  // сумма
  if (filterMinAmount.value !== null) params.append('minAmount', filterMinAmount.value.toString());
  if (filterMaxAmount.value !== null) params.append('maxAmount', filterMaxAmount.value.toString());

  // срок
  if (filterMinTerm.value !== null) params.append('minTerm', filterMinTerm.value.toString());
  if (filterMaxTerm.value !==null) params.append('maxTerm',filterMaxTerm.value.toString());

  // запрос на веб-апи
  const url = `http://localhost:5007/api/loans?${params.toString()}`;
  const res = await fetch(url);
  loans.value = await res.json();
}

onMounted(fetchLoans);

</script>

<style scoped src="@/assets/styles/layout.css"></style>
