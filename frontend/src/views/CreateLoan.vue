<template>
  <h2 class="create-loan">Новая заявка</h2>

  <div class="loan-form">
    <div class="form-row">
      <label>Номер заявки:</label>
      <input v-model="number" type="text" />
      <span v-if="errors.number" class="error">{{ errors.number }}</span>
    </div>

    <div class="form-row">
      <label>Сумма заявки:</label>
      <input v-model.number="amount" type="number" />
      <span v-if="errors.amount" class="error">{{ errors.amount }}</span>
    </div>

    <div class="form-row">
      <label>Срок займа:</label>
      <input v-model.number="termValue" type="number" />
      <span v-if="errors.termValue" class="error">{{ errors.termValue }}</span>
    </div>

    <div class="form-row">
      <label>Ставка(%):</label>
      <input v-model.number="interestValue" type="number" />
      <span v-if="errors.interestValue" class="error">{{ errors.interestValue }}</span>
    </div>

    <button class="b-create-loan" @click="createLoan">Создать заявку</button>
  </div>

</template>

<script setup lang="ts">
import router from '@/router'
import { ref } from 'vue'

// реактивные поля формы
const number = ref<string>('')
const amount = ref<number | null>(null)
const termValue = ref<number | null>(null)
const interestValue = ref<number | null>(null)

// ошибки валидации
const errors = ref<{ [key: string]: string }>({})

// функция валидации
function validate(): boolean {
  const messages: string[] = []

  if (!number.value.trim()) {
    messages.push('Номер заявки обязателен')
    
  }
  if (!amount.value || amount.value <= 0) {
    messages.push('Сумма должна быть больше 0')
    amount.value = null
  }
  if (!termValue.value || termValue.value <= 0) {
    messages.push('Срок займа должен быть больше 0')
    termValue.value = null
  }
  if (!interestValue.value || interestValue.value <= 0) {
    messages.push('Ставка должна быть больше 0')
    interestValue.value = null
  }

  if (messages.length > 0) {
    alert(messages.join('\n')) 
    return false
  }

  return true
}


// отправка POST-запроса
async function createLoan() {
  if (!validate()) return

  const payload = {
    status: 1,
    number: number.value,
    amount: amount.value,
    termValue: termValue.value,
    interestValue: interestValue.value
  }

  try {
    const response = await fetch('http://localhost:5007/api/Loans', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(payload)
    })

    if (!response.ok) {
      console.error('Ошибка при создании заявки:', response.status)
      return
    }

    const result = await response.json()
    console.log('Заявка создана:', result)
   
    // очистка формы
    number.value = ''
    amount.value = null
    termValue.value = null
    interestValue.value = null
    errors.value = {}
    //перенаправить на страницу заявки
    router.push('/loans')
    alert("Заявка успешно добавлена !")

  } catch (err) {
    console.error('Ошибка сети:', err)
  }
}
</script>


<style scoped src="@/assets/styles/layout.css"></style>
