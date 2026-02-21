import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { SubGrupoCuentaService, SubGrupoCuentaDTO } from '../../../services/subgrupo-cuenta.service';
import { GrupoCuentaService, GrupoCuentaDTO } from '../../../services/grupo-cuenta.service';
import { TranslationService } from '../../../services/translation.service';
import { LanguageService } from '../../../services/language.service';

@Component({
  selector: 'app-subgrupo-cuenta-form',
  templateUrl: './subgrupo-cuenta-form.component.html',
  styleUrls: ['./subgrupo-cuenta-form.component.css']
})
export class SubGrupoCuentaFormComponent implements OnInit, OnDestroy {
  form: FormGroup;
  isEditMode = false;
  subgrupoId: number | null = null;
  loading = false;
  loadingGrupos = true;
  error: string | null = null;
  gruposCuenta: GrupoCuentaDTO[] = [];

  selectedGrupoCodigo: string = '';
  codigoCompleto: string = '';

  private languageSubscription?: Subscription;

  constructor(
    private fb: FormBuilder,
    private subGrupoCuentaService: SubGrupoCuentaService,
    private grupoCuentaService: GrupoCuentaService,
    private router: Router,
    private route: ActivatedRoute,
    private translate: TranslationService,
    private languageService: LanguageService
  ) {
    this.form = this.fb.group({
      codigoUsuario: [
        { value: '', disabled: true },
        [Validators.required, Validators.pattern(/^\d{3$/), Validators.minLength(3), Validators.maxLength(3)]
      ],
      grupoCuentaId: [null, Validators.required],
      descripcion: ['', [Validators.required, Validators.maxLength(256)]],
      deudora: [true]
    });
  }

  ngOnInit(): void {
    this.loadGruposCuenta();

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.subgrupoId = +id;
    }

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        console.log('🔄 SubGrupoForm: Idioma cambiado, recargando datos...');
        this.reloadOnLanguageChange();
      });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
  }

  private reloadOnLanguageChange(): void {
    this.grupoCuentaService.getAll().subscribe({
      next: (data) => {
        this.gruposCuenta = data.filter(g => g.activo !== false);
        if (this.isEditMode && this.subgrupoId) {
          this.loadSubGrupoCuenta();
        }
      },
      error: (err: any) => {
        console.error('Error al recargar grupos:', err);
      }
    });
  }

  loadGruposCuenta(): void {
    this.loadingGrupos = true;

    this.grupoCuentaService.getAll().subscribe({
      next: (data) => {
        this.gruposCuenta = data.filter(g => g.activo !== false);
        this.loadingGrupos = false;

        this.setupFormSubscriptions();

        if (this.isEditMode && this.subgrupoId) {
          this.loadSubGrupoCuenta();
        }
      },
      error: (err: any) => {
        console.error('Error al cargar grupos:', err);
        this.error = this.translate.translate('error.loading');
        this.loadingGrupos = false;
      }
    });
  }

  private setupFormSubscriptions(): void {
    this.form.get('grupoCuentaId')?.valueChanges.subscribe(grupoId => {
      this.onGrupoCuentaChange(grupoId);
    });

    this.form.get('codigoUsuario')?.valueChanges.subscribe(() => {
      this.updateCodigoCompleto();
    });

    this.form.get('descripcion')?.valueChanges.subscribe(value => {
      if (value && typeof value === 'string' && value !== value.toUpperCase()) {
        this.form.get('descripcion')?.setValue(value.toUpperCase(), { emitEvent: false });
      }
    });
  }

  onGrupoCuentaChange(grupoId: number | null): void {
    if (!grupoId) {
      this.selectedGrupoCodigo = '';
      this.form.get('codigoUsuario')?.disable();
      this.form.get('codigoUsuario')?.setValue('');
      this.updateCodigoCompleto();
      return;
    }

    const grupoIdNumber = Number(grupoId);
    const grupoSeleccionado = this.gruposCuenta.find(g => Number(g.id) === grupoIdNumber);

    if (!grupoSeleccionado) {
      this.selectedGrupoCodigo = '';
      this.form.get('codigoUsuario')?.disable();
      this.form.get('codigoUsuario')?.setValue('');
      this.updateCodigoCompleto();
      return;
    }

    this.selectedGrupoCodigo = grupoSeleccionado.codigo || '';

    if (this.selectedGrupoCodigo) {
      this.form.get('codigoUsuario')?.enable();
    } else {
      this.form.get('codigoUsuario')?.disable();
      this.form.get('codigoUsuario')?.setValue('');
    }

    this.updateCodigoCompleto();
  }

  updateCodigoCompleto(): void {
    const codigoUsuario = this.form.get('codigoUsuario')?.value || '';

    if (this.selectedGrupoCodigo && codigoUsuario && codigoUsuario.length === 3) {
      this.codigoCompleto = this.selectedGrupoCodigo + codigoUsuario;
    } else {
      this.codigoCompleto = this.selectedGrupoCodigo ? `${this.selectedGrupoCodigo}___` : '';
    }
  }

  loadSubGrupoCuenta(): void {
    if (!this.subgrupoId) return;

    this.loading = true;
    this.subGrupoCuentaService.getById(this.subgrupoId).subscribe({
      next: (data) => {
        const codigoGrupo = data.codigo.substring(0, 2);
        const codigoUsuario = data.codigo.substring(2, 5);

        const grupo = this.gruposCuenta.find(g => g.codigo === codigoGrupo);

        this.form.patchValue({
          grupoCuentaId: grupo?.id,
        });

        setTimeout(() => {
          this.form.patchValue({
            codigoUsuario: codigoUsuario,
            descripcion: data.descripcion,
            deudora: data.deudora
          });

          if (this.isEditMode) {
            this.form.get('grupoCuentaId')?.disable();
            this.form.get('codigoUsuario')?.disable();
          }
        }, 100);

        this.loading = false;
      },
      error: (err: any) => {
        this.error = this.translate.translate('error.loading');
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  onSubmit(): void {
    const codigoUsuario = this.form.get('codigoUsuario')?.value;

    if (this.form.invalid || !this.codigoCompleto || !codigoUsuario || codigoUsuario.length !== 3) {
      this.form.markAllAsTouched();

      if (!codigoUsuario || codigoUsuario.length !== 3) {
        this.form.get('codigoUsuario')?.markAsTouched();
      }
      return;
    }

    this.loading = true;
    this.error = null;

    const rawValue = this.form.getRawValue();
    const dto: SubGrupoCuentaDTO = {
      ...rawValue,
      codigo: this.codigoCompleto
    };

    if (this.isEditMode) {
      this.subGrupoCuentaService.update(this.subgrupoId!, dto).subscribe({
        next: () => {
          this.router.navigate(['/nomencladores/subgrupo-cuenta']);
        },
        error: (err: any) => {
          this.error = this.translate.translate('error.saving');
          this.loading = false;
          console.error('Error:', err);
        }
      });
    } else {
      this.subGrupoCuentaService.create(dto).subscribe({
        next: () => {
          this.router.navigate(['/nomencladores/subgrupo-cuenta']);
        },
        error: (err: any) => {
          this.error = this.translate.translate('error.saving');
          this.loading = false;
          console.error('Error:', err);
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/nomencladores/subgrupo-cuenta']);
  }

  getErrorMessage(fieldName: string): string {
    const control = this.form.get(fieldName);
    if (control?.hasError('required')) {
      return this.translate.translate('validation.required');
    }
    if (control?.hasError('maxlength')) {
      const maxLength = control.errors?.['maxlength'].requiredLength;
      return this.translate.translate('validation.maxLength', [maxLength]);
    }
    if (control?.hasError('pattern') || control?.hasError('minlength')) {
      return this.translate.translate('validation.codigo3Digitos');
    }
    return '';
  }
}
