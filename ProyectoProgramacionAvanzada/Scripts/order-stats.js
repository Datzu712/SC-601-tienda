import { DataFetcher } from './modules/api.js';

const initialState = { period: 'month' };

let statusChart = null;
let salesChart = null;

/**
 * Update graphs with new data
 * @param { OrderStats } data - New data to update the charts
 */
function updateCharts(data) {
    document.getElementById('totalSales').textContent = `₡${data.totalSales.toLocaleString('es-CR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`;
    document.getElementById('totalOrders').textContent = data.totalOrders;
    
    const statusCtx = document.getElementById('statusChart').getContext('2d');
    
    const statusData = {
        labels: data.ordersByStatus.map(item => item.Estado),
        datasets: [{
            label: 'Órdenes por Estado',
            data: data.ordersByStatus.map(item => item.Count),
            backgroundColor: [
                'rgba(255, 193, 7, 0.6)',   // Warning - Pendiente
                'rgba(13, 202, 240, 0.6)',  // Info - Procesando
                'rgba(13, 110, 253, 0.6)',  // Primary - Enviado
                'rgba(25, 135, 84, 0.6)',   // Success - Entregado
                'rgba(220, 53, 69, 0.6)'    // Danger - Cancelado
            ],
            borderColor: [
                'rgba(255, 193, 7, 1)',
                'rgba(13, 202, 240, 1)',
                'rgba(13, 110, 253, 1)',
                'rgba(25, 135, 84, 1)',
                'rgba(220, 53, 69, 1)'
            ],
            borderWidth: 1
        }]
    };

    if (statusChart) {
        statusChart.destroy();
    }

    statusChart = new Chart(statusCtx, {
        type: 'doughnut',
        data: statusData,
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'bottom',
                },
                title: {
                    display: true,
                    text: 'Distribución de Órdenes por Estado',
                    font: {
                        size: 16
                    }
                }
            }
        }
    });
    
    const salesCtx = document.getElementById('salesChart').getContext('2d');
    
    const salesData = {
        labels: data.salesByPeriod.map(item => item.Fecha),
        datasets: [{
            label: 'Ventas (₡)',
            data: data.salesByPeriod.map(item => item.Total),
            backgroundColor: 'rgba(13, 110, 253, 0.2)',
            borderColor: 'rgba(13, 110, 253, 1)',
            borderWidth: 2,
            fill: true,
            tension: 0.3
        }]
    };

    if (salesChart) {
        salesChart.destroy();
    }

    salesChart = new Chart(salesCtx, {
        type: 'line',
        data: salesData,
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false
                },
                title: {
                    display: true,
                    text: 'Ventas en el Período',
                    font: {
                        size: 16
                    }
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        callback: function(value) {
                            return '₡' + value.toLocaleString('es-CR');
                        }
                    }
                }
            }
        }
    });
}

const { data } = new DataFetcher(initialState, {
    endpoint: '/Order/GetOrderStats',
    onSuccess: (data) => {
        updateCharts(data)
    },
    onError: (error) => {
        console.error(error);
        window.App.notifications.showToast('Error al cargar las estadísticas de órdenes.', { type: 'error' });
    },
    retry: 3,
    retryDelay: 1000,
    fetchOnMount: true,
    fetchOnWindowFocus: false
});

document.addEventListener('DOMContentLoaded', () => {
    const periodButtons = document.querySelectorAll('[data-period]');
    
    periodButtons.forEach(button => {
        button.addEventListener('click', (e) => {
            e.preventDefault();
            const period = button.getAttribute('data-period');
            
            periodButtons.forEach((btn) => btn.classList.remove('active'));
            button.classList.add('active');
            
            data.period = period;
        });
    });
});

