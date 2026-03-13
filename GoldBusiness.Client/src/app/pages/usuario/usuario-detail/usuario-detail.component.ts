import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { UsuarioDTO, UsuarioService } from '../../../services/usuario.service';

@Component({
  selector: 'app-usuario-detail',
  templateUrl: './usuario-detail.component.html',
  styleUrls: ['./usuario-detail.component.css']
})
export class UsuarioDetailComponent implements OnInit {
  item: UsuarioDTO | null = null;
  loading = false;
  error: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private usuarioService: UsuarioService,
    private location: Location
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadItem(id);
    }
  }

  loadItem(id: string): void {
    this.loading = true;
    this.error = null;

    this.usuarioService.getById(id).subscribe({
      next: (res) => {
        this.item = res;
        this.loading = false;
      },
      error: (err: Error) => {
        this.error = err.message || 'Error al cargar usuario';
        this.loading = false;
      }
    });
  }

  goBack(): void {
    this.location.back();
  }
}
