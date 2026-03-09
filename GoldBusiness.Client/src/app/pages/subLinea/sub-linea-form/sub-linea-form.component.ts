import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { SubLineaService, SubLineaDTO } from '../../../services/sub-linea.service';
import { LanguageService } from '../../../services/language.service';
import { TranslationService } from '../../../services/translation.service';
import { LineaService, LineaDTO } from '../../../services/linea.service';

@Component({
  selector: 'app-sub-linea-form',
  templateUrl: './sub-linea-form.component.html',
  styleUrls: ['./sub-linea-form.component.css']
})
export class SubLineaFormComponent implements OnInit, OnDestroy {
  form: FormGroup;
  isEditMode = false;
  subLineaId: number | null = null;
  loading = false;
  error: string | null = null;

  lineas: LineaDTO[] = [];
  loadingLineas = true;

  selectedLineaCodigo: string = '';
  codigoCompleto: string = '';

  private languageSubscription?: Subscription;

  constructor(
    private fb: FormBuilder,
    private subLineaService: SubLineaService,
    private lineaService: LineaService,
    private router: Router,
    private route: ActivatedRoute,
    private translate: TranslationService,
    private languageService: LanguageService
  ) {
    this.form = this.fb.group({
      // readonly calculated code
      codigo: [{ value: '', disabled: true }],
      codigoUsuario: [
        { value: '', disabled: true },
        [Validators.required, Validators.pattern(/^\d{3}$/), Validators.minLength(3), Validators.maxLength(3)]
      ],
      lineaId: [null, Validators.required],
      descripcion: ['', [Validators.required, Validators.maxLength(256)]]
    });
  }

  ngOnInit(): void {
    this.setupFormSubscriptions();
    this.loadLineas();

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.subLineaId = +id;
    }

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        this.loadLineas();
      });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
  }

  private setupFormSubscriptions(): void {
    // When a linea changes, set selected prefix and enable codigoUsuario
    this.form.get('lineaId')?.valueChanges.subscribe(lineaId => {
      this.onLineaChange(lineaId);
    });

    // When codigoUsuario changes, update composed code
    this.form.get('codigoUsuario')?.valueChanges.subscribe(() => {
      this.updateCodigoCompleto();
    });

    // Auto uppercase descripcion
    this.form.get('descripcion')?.valueChanges.subscribe(value => {
      if (value && typeof value === 'string' && value !== value.toUpperCase()) {
        this.form.get('descripcion')?.setValue(value.toUpperCase(), { emitEvent: false });
      }
    });
  }

  loadLineas(): void {
    this.loadingLineas = true;
    this.lineaService.getAll().subscribe({
      next: (data) => {
        this.lineas = data.filter(l => !l.cancelado);
        this.loadingLineas = false;

        if (this.isEditMode && this.subLineaId) {
          this.loadSubLinea();
        }
      },
      error: (err) => {
        this.error = 'Error al cargar líneas';
        this.loadingLineas = false;
      }
    });
  }

  onLineaChange(lineaId: number | null): void {
    if (!lineaId) {
      this.selectedLineaCodigo = '';
      this.form.get('codigoUsuario')?.disable();
      this.form.get('codigoUsuario')?.setValue('');
      this.updateCodigoCompleto();
      return;
    }

    const lineaIdNumber = Number(lineaId);
    const lineaSeleccionada = this.lineas.find(l => Number(l.id) === lineaIdNumber);

    if (!lineaSeleccionada) {
      this.selectedLineaCodigo = '';
      this.form.get('codigoUsuario')?.disable();
      this.form.get('codigoUsuario')?.setValue('');
      this.updateCodigoCompleto();
      return;
    }

    this.selectedLineaCodigo = lineaSeleccionada.codigo || '';

    if (this.selectedLineaCodigo) {
      this.form.get('codigoUsuario')?.enable();
    } else {
      this.form.get('codigoUsuario')?.disable();
      this.form.get('codigoUsuario')?.setValue('');
    }

    this.updateCodigoCompleto();
  }

  updateCodigoCompleto(): void {
    const codigoUsuario = this.form.get('codigoUsuario')?.value || '';

    if (this.selectedLineaCodigo && codigoUsuario && codigoUsuario.length === 3) {
      this.codigoCompleto = this.selectedLineaCodigo + codigoUsuario;
    } else {
      this.codigoCompleto = this.selectedLineaCodigo ? `${this.selectedLineaCodigo}___` : '';
    }
  }

  loadSubLinea(): void {
    if (!this.subLineaId) return;

    this.loading = true;
    this.subLineaService.getById(this.subLineaId).subscribe({
      next: (data) => {
        // ✅ Validar que data.codigo exista
        if (!data.codigo || data.codigo.length < 5) {
          this.error = 'Código de sublínea inválido';
          this.loading = false;
          return;
        }

        // Descomponer el código: 2 dígitos linea + 3 dígitos usuario
        const codigoLinea = data.codigo.substring(0, 2);
        const codigoUsuario = data.codigo.substring(2, 5);

        const linea = this.lineas.find(l => l.codigo === codigoLinea);

        this.form.patchValue({
          lineaId: linea?.id,
        });

        setTimeout(() => {
          this.form.patchValue({
            codigoUsuario: codigoUsuario,
            descripcion: data.descripcion
          });

          if (this.isEditMode) {
            this.form.get('lineaId')?.disable();
            this.form.get('codigoUsuario')?.disable();
          }
        }, 100);

        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar la sublínea';
        this.loading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.form.invalid || !this.codigoCompleto) {
      Object.keys(this.form.controls).forEach(key => {
        this.form.get(key)?.markAsTouched();
      });
      return;
    }

    this.loading = true;
    this.error = null;

    const formData: SubLineaDTO = {
      ...this.form.getRawValue(),
      codigo: this.codigoCompleto
    };

    if (this.isEditMode) {
      this.subLineaService.update(this.subLineaId!, formData).subscribe({
        next: () => {
          this.router.navigate(['/nomencladores/sub-linea']);
        },
        error: (err) => {
          this.loading = false;
          this.error = 'Error al actualizar';
        }
      });
    } else {
      this.subLineaService.create(formData).subscribe({
        next: () => {
          this.router.navigate(['/nomencladores/sub-linea']);
        },
        error: (err) => {
          this.loading = false;
          this.error = 'Error al crear';
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/nomencladores/sub-linea']);
  }

  getErrorMessage(fieldName: string): string {
    const control = this.form.get(fieldName);
    if (!control || !control.errors) return '';

    const errors = control.errors;
    if (errors['required']) return this.translate.translate('validation.required');
    if (errors['pattern']) return this.translate.translate('validation.numeric');
    if (errors['minlength'] || errors['maxlength']) return this.translate.translate('validation.length');

    return this.translate.translate('validation.invalid');
  }
}
