import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GrupoCuentaService, GrupoCuentaDTO } from '../../../services/grupo-cuenta.service';

@Component({
  selector: 'app-grupo-cuenta-detail',
  templateUrl: './grupo-cuenta-detail.component.html',
  styleUrls: ['./grupo-cuenta-detail.component.css']
})
export class GrupoCuentaDetailComponent implements OnInit {
  grupo: GrupoCuentaDTO | null = null;
  id: number | null = null; // ✅ DEBE EXISTIR
  loading = true;
  error: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private grupoCuentaService: GrupoCuentaService
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.id = +idParam;
      this.loadGrupoCuenta();
    }
  }

  loadGrupoCuenta(): void {
    this.loading = true;
    this.error = null;

    this.grupoCuentaService.getById(this.id!).subscribe({
      next: (data) => {
        this.grupo = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar el grupo de cuenta';
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  goToEdit(): void {
    this.router.navigate(['/nomencladores/grupo-cuenta/editar', this.id]);
  }

  goBack(): void {
    this.router.navigate(['/nomencladores/grupo-cuenta']);
  }

  delete(): void {
    if (!confirm(`¿Está seguro que desea eliminar el grupo "${this.grupo?.descripcion}"?`)) {
      return;
    }

    this.grupoCuentaService.delete(this.id!).subscribe({
      next: () => {
        this.router.navigate(['/nomencladores/grupo-cuenta']);
      },
      error: (err) => {
        this.error = err.error?.message || 'Error al eliminar el grupo de cuenta';
        console.error('Error:', err);
      }
    });
  }
}
