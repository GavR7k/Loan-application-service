<template>
  <div class="loan-list">
    <h1>Список заявок</h1>
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

onMounted(async () => {
  const res = await fetch('http://localhost:5007/api/loans');
  loans.value = await res.json();
});

function formatDate(dateStr: string): string {
  const date = new Date(dateStr);
  return date.toLocaleDateString('ru-RU');
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

</script>

<style scoped src="@/assets/styles/layout.css"></style>
