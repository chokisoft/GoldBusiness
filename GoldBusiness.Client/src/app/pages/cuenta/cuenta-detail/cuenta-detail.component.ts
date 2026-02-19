import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CuentaService, CuentaDTO } from '../../../services/cuenta.service';

@Component({
  selector: 'app-cuenta-detail',
  templateUrl: './cuenta-detail.component.html',
  styleUrls: ['./cuenta-detail.component.css']
})
export class CuentaDetailComponent implements OnInit {
  cuenta: CuentaDTO | null = null;
  id: number | null = null; // ✅ AGREGAR ESTA PROPIEDAD
  loading = true;
  error: string | null = null;

  constructor(
    private cuentaService: CuentaService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    // ✅ Obtener ID de la ruta
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.id = +idParam;
      this.loadCuenta();
    } else {
      this.error = 'ID no válido';
      this.loading = false;
    }
  }

  loadCuenta(): void {
    if (!this.id) return;

    this.loading = true;
    this.cuentaService.getById(this.id).subscribe({
      next: (data) => {
        this.cuenta = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar la cuenta';
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  // ✅ Método para volver
  goBack(): void {
    this.router.navigate(['/nomencladores/cuenta']);
  }
}
