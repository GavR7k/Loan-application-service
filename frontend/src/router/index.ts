import { createRouter, createWebHistory } from 'vue-router'
import LoanList from '@/views/LoanList.vue'
import CreateLoan from '@/views/CreateLoan.vue'

const router = createRouter({
    history: createWebHistory(),
    routes: [
        { path: '/', redirect: '/loans' },
        { path: '/loans', component: LoanList },
        { path: '/loans/create', component: CreateLoan }
    ]
});

export default router;