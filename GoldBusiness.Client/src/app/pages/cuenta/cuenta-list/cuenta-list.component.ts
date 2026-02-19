import { Component, OnInit } from '@angular/core';
import { CuentaService, CuentaDTO } from '../../../services/cuenta.service';

@Component({
  selector: 'app-cuenta-list',
  templateUrl: './cuenta-list.component.html',
  styleUrls: ['./cuenta-list.component.css']
})
export class CuentaListComponent implements OnInit {
  cuentas: CuentaDTO[] = [];
  loading = false;
  error: string | null = null;

  constructor(private cuentaService: CuentaService) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.loading = true;
    this.error = null;

    this.cuentaService.getAll().subscribe({
      next: (data) => {
        this.cuentas = data.sort((a, b) => a.codigo.localeCompare(b.codigo));
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar las cuentas';
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  delete(id: number, descripcion: string): void {
    if (confirm(`¿Está seguro de eliminar la cuenta "${descripcion}"?`)) {
      this.cuentaService.delete(id).subscribe({
        next: () => {
          this.loadData();
        },
        error: (err) => {
          this.error = 'Error al eliminar la cuenta';
          console.error('Error:', err);
        }
      });
    }
  }
}
