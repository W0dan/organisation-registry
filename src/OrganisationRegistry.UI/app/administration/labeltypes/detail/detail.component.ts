import {Component, ElementRef, OnDestroy, OnInit} from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { FormBuilder, FormGroup } from '@angular/forms';

import { AlertService } from 'core/alert';
import { CreateAlertMessages, UpdateAlertMessages } from 'core/alertmessages';
import { Create, ICrud, Update } from 'core/crud';
import { required } from 'core/validation';

import { LabelType, LabelTypeService } from 'services/labeltypes';
import {Subscription} from "rxjs/Subscription";

@Component({
  templateUrl: 'detail.template.html',
  styleUrls: ['detail.style.css']
})
export class LabelTypeDetailComponent implements OnInit, OnDestroy {
  public isEditMode: boolean;
  public form: FormGroup;

  private crud: ICrud<LabelType>;
  private readonly createAlerts = new CreateAlertMessages('Benaming type');
  private readonly updateAlerts = new UpdateAlertMessages('Benaming type');

  private readonly subscriptions: Subscription[] = new Array<Subscription>();

  get isFormValid() {
    return this.form.enabled && this.form.valid;
  }

  constructor(
    formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private alertService: AlertService,
    private itemService: LabelTypeService
  ) {
    this.form = formBuilder.group({
      id: ['', required],
      name: ['', required]
    });
  }

  ngOnInit() {
    this.route.params.forEach((params: Params) => {
      this.form.disable();

      let id = params['id'];
      this.isEditMode = id !== null && id !== undefined;

      this.crud = this.isEditMode
        ? new Update<LabelTypeService, LabelType>(id, this.itemService, this.alertService, this.updateAlerts)
        : new Create<LabelTypeService, LabelType>(this.itemService, this.alertService, this.createAlerts);

      this.subscriptions.push(this.crud
        .load(LabelType)
        .finally(() => this.form.enable())
        .subscribe(
          item => {
            if (item)
              this.form.setValue(item);
          },
          error => this.crud.alertLoadError(error)));
    });
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  createOrUpdate(value: LabelType) {
    this.form.disable();

    this.subscriptions.push(this.crud.save(value)
      .finally(() => this.form.enable())
      .subscribe(
        result => {
          if (result) {
            let labelTypeUrl = this.router.serializeUrl(
              this.router.createUrlTree(
                ['./../', value.id],
                { relativeTo: this.route }));

            this.router.navigate(['./..'], { relativeTo: this.route });

            this.crud.alertSaveSuccess(value, labelTypeUrl);
          }
        },
        error => this.crud.alertSaveError(error)));
  }
}
