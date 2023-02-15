import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CdkTreeModule, FlatTreeControl } from '@angular/cdk/tree';
import { NestedTreeControl } from '@angular/cdk/tree';
import { MatTreeNestedDataSource } from '@angular/material/tree';
import { ShopService } from '../shop.service'
import { map, Observable, of, ReplaySubject } from 'rxjs';
import { ICategory, ICategoryTree } from 'src/app/shared/models/productCategory';

@Component({
  selector: 'app-productcategoriestree',
  templateUrl: './product-categories-tree.component.html',
  styleUrls: ['./product-categories-tree.component.scss']
})
export class ProductCategoriesTreeComponent implements OnInit {

  @Output() categorySelected: EventEmitter<string> = new EventEmitter<string>();

  treeControl: NestedTreeControl<ICategoryTree>;
  dataSource: MatTreeNestedDataSource<ICategoryTree>;


  constructor(private shopService: ShopService) {
    this.treeControl = new NestedTreeControl<ICategoryTree>(node => of(node.children));
    this.dataSource = new MatTreeNestedDataSource<ICategoryTree>();
  }

  ngOnInit(): void {
    this.shopService.getCategoriesTree().subscribe({
      next: categories => {
        this.dataSource.data = categories;
      }
    });

  }

  hasChild = (nodeData: ICategoryTree) => { return (nodeData.children); };

  nodeClicked(node) {
    if (this.treeControl.isExpanded(node)) {
      // let parent = null;

      console.log(node);

      this.categorySelected.emit(node.id);
      
      // this.shopService.shopParams.categoryId = node.id;
      // this.shopService.getProducts(false);
    //   let index = this.treeControl.dataNodes.findIndex((n) => n.id === node.id);

    //   for (let i = index; i >= 0; i--) {
    //     if (node.level > this.treeControl.dataNodes[i].level) {
    //       parent = this.treeControl.dataNodes[i];
    //       break;
    //     }
    //   }

    //   if (parent) {
    //     this.treeControl.collapseDescendants(parent);
    //     this.treeControl.expand(parent);
    //   } else {
    //     this.treeControl.collapseAll()
    //   }
    //   this.treeControl.expand(node);
    }
  }

}
