import { Component, OnInit } from '@angular/core';
import { GrupoCuentaService, GrupoCuentaDTO } from '../../../services/grupo-cuenta.service';

@Component({
  selector: 'app-grupo-cuenta-list',
  templateUrl: './grupo-cuenta-list.component.html',
  styleUrls: ['./grupo-cuenta-list.component.css']
})
export class GrupoCuentaListComponent implements OnInit {
  gruposCuenta: GrupoCuentaDTO[] = [];
  loading = false;
  error: string | null = null;

  constructor(private grupoCuentaService: GrupoCuentaService) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.loading = true;
    this.error = null;

    this.grupoCuentaService.getAll().subscribe({
      next: (data) => {
        this.gruposCuenta = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar los grupos de cuenta';
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  delete(id: number, nombre: string): void {
    if (confirm(`¿Está seguro de eliminar el grupo "${nombre}"?`)) {
      this.grupoCuentaService.delete(id).subscribe({
        next: () => {
          this.loadData();
        },
        error: (err) => {
          this.error = 'Error al eliminar el grupo de cuenta';
          console.error('Error:', err);
        }
      });
    }
  }
}
