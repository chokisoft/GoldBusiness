import { Component } from '@angular/core';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-test-connection',
  templateUrl: './test-connection.component.html',
  styleUrls: ['./test-connection.component.css']
})
export class TestConnectionComponent {
  apiResponse: any = null;
  isLoading = false;
  error: string | null = null;

  constructor(private apiService: ApiService) {}

  /**
   * Probar conexión con la API
   */
  testConnection(): void {
    this.isLoading = true;
    this.error = null;
    this.apiResponse = null;

    // Probar el endpoint de información de la API
    this.apiService.get<any>('info').subscribe({
      next: (response) => {
        console.log('✅ Conexión exitosa:', response);
        this.apiResponse = response;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('❌ Error de conexión:', error);
        this.error = error.message;
        this.isLoading = false;
      }
    });
  }
}
