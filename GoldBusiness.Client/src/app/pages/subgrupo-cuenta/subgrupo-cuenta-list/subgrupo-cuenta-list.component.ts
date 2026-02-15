import { Component, OnInit } from '@angular/core';
import { SubGrupoCuentaService, SubGrupoCuentaDTO } from '../../../services/subgrupo-cuenta.service';

@Component({
  selector: 'app-subgrupo-cuenta-list',
  templateUrl: './subgrupo-cuenta-list.component.html',
  styleUrls: ['./subgrupo-cuenta-list.component.css']
})
export class SubGrupoCuentaListComponent implements OnInit {
  subgrupos: SubGrupoCuentaDTO[] = [];
  loading = false;
  error: string | null = null;

  constructor(private subGrupoCuentaService: SubGrupoCuentaService) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.loading = true;
    this.error = null;

    this.subGrupoCuentaService.getAll().subscribe({
      next: (data) => {
        this.subgrupos = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar los subgrupos de cuenta';
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  delete(id: number, descripcion: string): void {
    if (confirm(`¿Está seguro de eliminar el subgrupo "${descripcion}"?`)) {
      this.subGrupoCuentaService.delete(id).subscribe({
        next: () => {
          this.loadData();
        },
        error: (err) => {
          this.error = 'Error al eliminar el subgrupo de cuenta';
          console.error('Error:', err);
        }
      });
    }
  }
}
